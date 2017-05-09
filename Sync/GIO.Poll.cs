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
        public Thread Poller;
        static Queue<Operation> Queue = new Queue<Operation>();

        void Poll()
        {
            while (true)
            {
                bool changeMade = false;
                if (Service != null)
                {
                    string DriveFolderID;
                    GetSettings().TryGetValue("folder", out DriveFolderID);
                    if (AppData.Files != null &&
                        DriveFolderID != null &&
                        Program.Watcher.EnableRaisingEvents)
                    {
                        if (AppData.PageToken == null) AppData.PageToken = Service.Changes.GetStartPageToken().Execute().StartPageTokenValue;
                        string pageToken = AppData.PageToken;
                        while (pageToken != null)
                        {
                            var request = Service.Changes.List(pageToken);
                            request.Fields = "changes(time,file(id,name,parents,trashed,md5Checksum,appProperties,mimeType)), newStartPageToken, nextPageToken";
                            request.RestrictToMyDrive = true;
                            request.Spaces = "drive";
                            var changes = request.Execute();
                            if (changes.Changes.Count > 0) using (var Noti = Program.AddOngoing(0, StateCode.Pending, "Synchronising local folder"))
                                    foreach (var change in changes.Changes) /*try*/
                                    {
                                        if (change?.File?.Md5Checksum == null && change.File?.MimeType != FolderMIME) continue;

                                        var path = AppData.Files.FirstOrDefault(f => f.Value.ID == change.File.Id).Key;
                                        var parentPath = Path.GetDirectoryName(path);
                                        var driveParentPath = AppData.Files.FirstOrDefault(f => f.Value.ID == change?.File?.Parents[0]).Key;
                                        if (driveParentPath == null) continue;

                                        if (path == null)
                                        {
                                            if (change.File.Trashed == true) continue;
                                            Download(change.File, driveParentPath);
                                            changeMade = true;
                                            continue;
                                        }
                                        if (AppData.Trashed.Contains(path) && change.File.Trashed == false)
                                            using (var noti = Program.AddOngoing(0, StateCode.Pending, "Untrashing " + change.File.Name))
                                            {
                                                if (Directory.Exists(path)) Directory.Delete(path, true);
                                                if (File.Exists(path)) File.Delete(path);
                                                AppData.Trashed.Remove(path);
                                                if (Utils.RestoreItem(path)) SyncLocal(path);
                                                else Download(change.File, parentPath);
                                                changeMade = true;
                                                continue;
                                            }
                                        if (change.File.Trashed == true || change.Removed == true && !AppData.Trashed.Contains(path))
                                            using (var noti = Program.AddOngoing(0, StateCode.Pending, "Trashing " + change.File.Name))
                                            {
                                                if (change.File.Md5Checksum != AppData.Files[path].MD5) continue;
                                                if (change.Removed == true)
                                                {
                                                    AppData.Trashed.Add(path);
                                                    AppData.Files.Remove(path);
                                                }
                                                else AppData.Trashed.Add(path);
                                                Utils.RecyclingBin.MoveHere(path);
                                                continue;
                                            }

                                        var drivePath = Path.Combine(driveParentPath, change.File.Name);

                                        if (change.File.MimeType == FolderMIME)
                                        {
                                            if (change.File.Name != new DirectoryInfo(path).Name ||
                                                AppData.Files.ContainsKey(parentPath) &&
                                                change.File.Parents[0] != AppData.Files[parentPath].ID)
                                            {
                                                validatePath(ref drivePath, change.File);
                                                string action;
                                                if (change.File.Parents[0] == AppData.Files[parentPath].ID)
                                                {
                                                    if (Path.GetFileName(path) == change.File.Name) continue;
                                                    action = "Renaming ";
                                                }
                                                else action = "Moving ";
                                                using (var noti = Program.AddOngoing(0, StateCode.Pending, action + Path.GetFileName(path)))
                                                {
                                                    AppData.Files[drivePath] = AppData.Files[path];
                                                    AppData.Files.Remove(path);
                                                    if (Directory.Exists(path)) Directory.Move(path, drivePath);
                                                    else Download(change.File, drivePath);
                                                    changeMade = true;
                                                }
                                            }
                                        }

                                        else if (change.File.Name != Path.GetFileName(path) || change.File.Parents[0] != AppData.Files[parentPath].ID)
                                        {
                                            validatePath(ref drivePath, change.File);
                                            string action;
                                            if (change.File.Parents[0] == AppData.Files[parentPath].ID)
                                            {
                                                if (Path.GetFileName(path) == change.File.Name) continue;
                                                action = "Renaming ";
                                            }
                                            else action = "Moving ";
                                            using (var noti = Program.AddOngoing(0, StateCode.Pending, action + Path.GetFileName(path)))
                                            {
                                                AppData.Files[drivePath] = AppData.Files[path];
                                                AppData.Files.Remove(path);
                                                if (File.Exists(path)) File.Move(path, drivePath);
                                                else using (var stream = new FileStream(drivePath, FileMode.Create, FileAccess.Write))
                                                        Service.Files.Get(change.File.Id).Download(stream);
                                                changeMade = true;
                                            }
                                        }
                                        else if (change.File.Md5Checksum != AppData.Files[path].MD5)
                                            using (var noti = Program.AddOngoing(0, StateCode.Pending, "Applying changes to " + change.File.Name))
                                            {
                                                UpdateProperties(change.File.Id, "lastMd5", change.File.Md5Checksum);
                                                AppData.Files[path].MD5 = change.File.Md5Checksum;
                                                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                                                    Service.Files.Get(change.File.Id).Download(stream);
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

                            if (changes.NewStartPageToken != null) AppData.PageToken = changes.NewStartPageToken;
                            pageToken = changes.NextPageToken;
                        }
                    }
                    if (Queue.Count > 0)
                    {
                        while (Queue.Count > 0)
                        {
                            var operation = Queue.Dequeue();

                            operation.Action();
                            if (operation.Cancel.HasValue) operation.Cancel.Value.Dispose();
                        }
                        changeMade = true;
                    }
                    if (changeMade) AppData.Save();
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
