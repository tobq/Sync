using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


namespace Sync
{
    partial class GIO
    {
        static public Thread Poller;
        static Queue<Operation> Queue = new Queue<Operation>();
        static TimeSpan[] QuietHours = new TimeSpan[2];
        static StateNotification? SleepNoti;
        static bool patchNeeded;
        public static bool QuietTime;

        void Poll()
        {
            while (true)
            {
                patchNeeded = QuietTime;
                bool changeMade = false;
                _File DriveFolderFile = null;

                var currentTtime = DateTime.Now.TimeOfDay;
                if (QuietHours[0] == null || QuietHours[1] == null) SetQuietHours();

                if (QuietHours[0] < QuietHours[1]) QuietTime = QuietHours[0] <= currentTtime && currentTtime <= QuietHours[1];
                else QuietTime = QuietHours[1] < currentTtime || currentTtime < QuietHours[0];

                if (patchNeeded == !QuietTime)
                {
                    if (QuietTime) SleepNoti = Program.AddOngoing(1, StateCode.Misc, "Quiet hours activated");
                    else
                    {
                        PatchRemote();
                        if (SleepNoti.HasValue) SleepNoti.Value.Dispose();
                    }
                }

                if (Service != null &&
                    AppData.Path != null &&
                    AppData.Files != null &&
                    !QuietTime &&
                    AppData.Files.TryGetValue(AppData.Path, out DriveFolderFile) &&
                    Program.Watcher.EnableRaisingEvents)
                {
                    var DriveFolderID = DriveFolderFile.ID;
                    if (AppData.PageToken == null)
                        AppData.PageToken = Service.Changes.GetStartPageToken().Execute().StartPageTokenValue;
                    string pageToken = AppData.PageToken;
                    while (pageToken != null)
                    {
                        var request = Service.Changes.List(pageToken);
                        request.Fields = "changes(time,file(id,name,parents,trashed,md5Checksum,appProperties,mimeType)), newStartPageToken, nextPageToken";
                        request.RestrictToMyDrive = true;
                        request.Spaces = "drive";
                        var changes = request.Execute();
                        foreach (var change in changes.Changes) /*try*/
                        {
                            if (change?.File?.Md5Checksum == null && change.File?.MimeType != FolderMIME)
                                continue;

                            var path = AppData.Files.ToList().FirstOrDefault(f => f.Value.ID == change.File.Id).Key;
                            var parentPath = Path.GetDirectoryName(path);
                            var driveParentPath = AppData.Files.FirstOrDefault(f => f.Value.ID == change?.File?.Parents[0]).Key;
                            if (driveParentPath == null)
                                continue;

                            if (path == null)
                            {
                                if (change.File.Trashed == true)
                                    continue;
                                Download(change.File, driveParentPath);
                                changeMade = true;
                                continue;
                            }
                            if (AppData.Trashed.Contains(path) && change.File.Trashed == false)
                                using (var noti = Program.AddOngoing(0, StateCode.Pending, "Untrashing " + change.File.Name))
                                {
                                    if (Directory.Exists(path))
                                        Directory.Delete(path, true);
                                    if (File.Exists(path))
                                        File.Delete(path);
                                    AppData.Trashed.Remove(path);
                                    if (Utils.RestoreItem(path) && File.Exists(path) && !Directory.Exists(path))
                                        SyncLocal(path);
                                    else
                                        Download(change.File, parentPath);
                                    changeMade = true;
                                    continue;
                                }
                            if (change.File.Trashed == true || change.Removed == true && !AppData.Trashed.Contains(path))
                                using (var noti = Program.AddOngoing(0, StateCode.Pending, "Trashing " + change.File.Name))
                                {
                                    if (change.File.Md5Checksum != AppData.Files[path].MD5)
                                        continue;
                                    if (change.Removed == true)
                                    {
                                        AppData.Trashed.Add(path);
                                        AppData.Files.Remove(path);
                                    }
                                    else
                                        AppData.Trashed.Add(path);
                                    Utils.RecyclingBin.MoveHere(path);
                                    continue;
                                }

                            var drivePath = Path.Combine(driveParentPath, change.File.Name);

                            if (path != drivePath)
                            {
                                validatePath(ref drivePath, change.File);
                                string action;
                                if (change.File.Parents[0] == AppData.Files[parentPath].ID)
                                    action = "Renaming ";
                                else
                                    action = "Moving ";
                                using (var noti = Program.AddOngoing(0, StateCode.Pending, action + Path.GetFileName(path)))
                                {
                                    AppData.Files[drivePath] = AppData.Files[path];
                                    AppData.Files.Remove(path);

                                    if (Directory.Exists(path))
                                        Directory.Move(path, drivePath);
                                    else if (File.Exists(path))
                                        File.Move(path, drivePath);
                                    else
                                        Download(change.File, driveParentPath);
                                    changeMade = true;
                                }
                            }

                            else if (change.File.Md5Checksum != AppData.Files[path].MD5)
                                using (var noti = Program.AddOngoing(0, StateCode.Pending, "Applying changes to " + change.File.Name))
                                {
                                    if (change.File.AppProperties["lastMd5"] != change.File.Md5Checksum)
                                        UpdateProperties(change.File.Id, "lastMd5", change.File.Md5Checksum);
                                    AppData.Files[path].MD5 = change.File.Md5Checksum;
                                    Download(change.File, parentPath);
                                }

                            if (!File.Exists(drivePath) &&
                                !Directory.Exists(drivePath) &&
                                !File.Exists(path) &&
                                !Directory.Exists(path)
                                && change.File.Trashed != true)
                            {
                                AppData.Trashed.Remove(path);
                                Download(change.File, driveParentPath);
                                changeMade = true;
                                continue;
                            }
                        }
                        //catch (IOException) { throw; }

                        if (changes.NewStartPageToken != null)
                            AppData.PageToken = changes.NewStartPageToken;
                        pageToken = changes.NextPageToken;
                    }
                }

                if (Queue.Count > 0 && !QuietTime)
                {
                    while (Queue.Count > 0)
                    {
                        bool completed = false;
                        var operation = Queue.Dequeue();

                        while (!completed)
                        {
                            try
                            {
                                operation.Action();
                                completed = true;
                                if (operation.Cancel.HasValue)
                                    operation.Cancel.Value.Dispose();
                            }
                            catch (Exception e)
                            {
                                using (var noti = Program.AddOngoing(operation.Cancel.Value.Folder, StateCode.Error, "Retrying in 2 seconds ..."))
                                    Thread.Sleep(2000);
                            }
                        }
                    }
                    changeMade = true;
                }
                if (changeMade) AppData.Save();
                Thread.Sleep(1000);
            }
        }
    }
}
