using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

namespace Sync
{
    static class Program
    {
        public static Color[] StateColours = new Color[] { Colours.red, Colours.blue, Colours.yellow, Colours.green };
        public static Icon[] StateIcons = new Icon[] { Properties.Resources._0, Properties.Resources._1, Properties.Resources._2, Properties.Resources._3 };
        public static Dictionary<long, StateInfo>[] Ongoing = new[] {
            new Dictionary<long, StateInfo>(),
            new Dictionary<long, StateInfo>()
        };

        public static List<string> RecentlyDeleted = new List<string>();

        public static GIO gio;
        public static Login Login;
        public static Settings Settings;
        public static Local Local;
        public static Drive Drive;
        public static Advanced Advanced;


        public delegate void StateChange(StateNotifcation? cancel = null);
        public static StateChange StateChanged;

        public static class Colours
        {
            public static Color blue = Color.FromArgb(72, 133, 237),
            red = Color.FromArgb(219, 50, 54),
            yellow = Color.FromArgb(244, 194, 13),
            green = Color.FromArgb(68, 212, 96),
            text = Color.FromArgb(160, 160, 160),
            grey = Color.FromArgb(241, 241, 241);
        };

        public static FileSystemWatcher Watcher = new FileSystemWatcher()
        {
            InternalBufferSize = 64000,
            IncludeSubdirectories = true
        };
        public static MD5 md5 = MD5.Create();
        static EventWaitHandle hevent;

        [STAThread]
        static void Main()
        {
            try
            {
                EventWaitHandle.OpenExisting("Sync#startup").Set();
                return;
            }
            catch
            {
                gio = new GIO();
                Login = new Login();
                Settings = new Settings();
                Local = new Local();
                Drive = new Drive();
                Advanced = new Advanced();
                bool created;
                hevent = new EventWaitHandle(false, EventResetMode.ManualReset, "Sync#startup", out created);
                if (created) new Thread(() =>
                {
                    while (true)
                    {
                        hevent.WaitOne();
                        if (gio.Logged) Settings.Invoke((MethodInvoker)delegate { Canvas.OpenSettings(); });
                        else Login.show();
                        hevent.Reset();
                    }
                }).Start();
                else
                {
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
            StateChanged += Settings.OnStateChanged;
            StateChanged += Canvas.UpdateTrayIcon;

            Watcher.Renamed += onRename;
            Watcher.Deleted += onDelete;
            Watcher.Created += onCreate;
            Watcher.Changed += onChange;


            if (gio.Logged)
            {
                Settings.SetImage();
                Settings.show();

                string folderID;
                if (AppData.Path == null) ClearLocal();
                else StateChanged(new StateNotifcation { Folder = 0 });
                if (gio.GetSettings().TryGetValue("folder", out folderID)) StateChanged(new StateNotifcation { Folder = 1 });
                else ClearDrive();
                AppData.Save();

                if (AppData.Files != null)
                {
                    if (Directory.Exists(AppData.Path))
                    {
                        Watcher.Path = AppData.Path;
                        Watcher.EnableRaisingEvents = true;
                    }
                    else ClearDrive();
                }
            }
            else Login.show();

            Application.EnableVisualStyles();
            Application.Run();
        }

        private static void onDelete(object sender, FileSystemEventArgs e)
        {
            if (AppData.Files.ContainsKey(e.FullPath) && !AppData.Trashed.Contains(e.FullPath) && !RecentlyDeleted.Contains(e.FullPath))
            {
                RecentlyDeleted.Add(e.FullPath);
                var timer = new System.Timers.Timer(500);
                timer.Elapsed += (S, E) =>
                {
                    if (RecentlyDeleted.Contains(e.FullPath))
                    {
                        RecentlyDeleted.Remove(e.FullPath);
                        AppData.Trashed.Remove(e.FullPath);
                        var trashID = AppData.Files[e.FullPath].ID;
                        AppData.Files.Remove(e.FullPath);

                        timer.Stop();
                        timer.Dispose();
                        GIO.QueueOperation(
                            () => gio.Trash(trashID, true),
                            AddOngoing(1, StateCode.Pending, $"Trashing {Path.GetFileName(e.FullPath)}")
                        );
                    }
                };
                timer.Start();
            }
        }

        static void onCreate(object s, FileSystemEventArgs e)
        {
            if (AppData.Files.ContainsKey(e.FullPath) && !AppData.Trashed.Contains(e.FullPath)) return;
            AppData.Trashed.Remove(e.FullPath);

            if (Directory.Exists(e.FullPath))
            {
                GIO.QueueOperation(
                    () => AppData.Files[e.FullPath] = new _File
                    {
                        ID = gio.NewFolder(new DirectoryInfo(e.FullPath).Name,
                        AppData.Files[Path.GetDirectoryName(e.FullPath)].ID)
                    },
                    AddOngoing(1, StateCode.Pending, $"Creating new folder {Path.GetFileName(e.FullPath)}")
                );
            }
            else
            {
                string checksum = "";
                var tries = 5;
                do
                {
                    try
                    {
                        checksum = Utils.MD5String(md5.ComputeHash(File.ReadAllBytes(e.FullPath)));
                        break;
                    }
                    catch (IOException) { Thread.Sleep(2000 / tries); }
                }
                while (--tries > 0);
                if (tries == 0)
                {
                    var UNRESOLVED = AddOngoing(0, StateCode.Error, $"Failed to apply changes for {Path.GetFileName(e.FullPath)} as the file could not be opened");
                    return;
                }
                foreach (var deletedKey in RecentlyDeleted.ToArray())
                {
                    if (AppData.Files[deletedKey].MD5 == checksum
                        && Path.GetFileName(deletedKey) == Path.GetFileName(e.FullPath))
                    {
                        RecentlyDeleted.Remove(deletedKey);
                        GIO.QueueOperation(
                            () => gio.Move(deletedKey, e.FullPath),
                            AddOngoing(1, StateCode.Pending, $"Moving {Path.GetFileName(e.FullPath)}")
                        );
                        return;
                    }
                }
                foreach (var trashedKey in AppData.Trashed.ToArray())
                {
                    if (AppData.Files.ContainsKey(trashedKey))
                    {
                        if (AppData.Files[trashedKey].MD5 == checksum
                            && Path.GetFileName(trashedKey) == Path.GetFileName(e.FullPath))
                        {
                            AppData.Trashed.Remove(trashedKey);
                            File.Delete(Path.Combine(AppData.TrashFolder, Path.GetFileName(trashedKey)));

                            GIO.QueueOperation(
                                () => gio.Trash(AppData.Files[trashedKey].ID, false),
                                AddOngoing(1, StateCode.Pending, $"Untrashing {Path.GetFileName(e.FullPath)}")
                            );
                            return;
                        }
                    }
                    else AppData.Trashed.Remove(trashedKey);
                }
                GIO.QueueOperation(
                    () => gio.Upload(e.FullPath, AppData.Files[Path.GetDirectoryName(e.FullPath)].ID),
                    AddOngoing(1, StateCode.Pending, $"Uploading {Path.GetFileName(e.FullPath)}"));
            }
        }

        private static void onRename(object sender, RenamedEventArgs e)
        {
            if (AppData.Files.ContainsKey(e.FullPath) && !AppData.Trashed.Contains(e.FullPath)) return;
            _File file;
            if (AppData.Files.TryGetValue(e.OldFullPath, out file))
            {
                AppData.Files[e.FullPath] = file;
                AppData.Files.Remove(e.OldFullPath);
                var newName = Path.GetFileName(e.FullPath);
                GIO.QueueOperation(
                    () => gio.Rename(file.ID, newName),
                    AddOngoing(1, StateCode.Pending, $"Renaming remote item to {newName}")
                );
            }
            else GIO.QueueOperation(
               () => gio.Upload(e.FullPath, AppData.Files[Path.GetDirectoryName(e.FullPath)].ID),
               AddOngoing(1, StateCode.Pending, $"Uploading {Path.GetFileName(e.FullPath)}")
           );
        }

        static void onChange(object s, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath)) return;
            string checksum = "";
            var tries = 5;
            do
            {
                try
                {
                    checksum = Utils.MD5String(md5.ComputeHash(File.ReadAllBytes(e.FullPath)));
                    break;
                }
                catch (IOException) { Thread.Sleep(2000 / tries); }
            }
            while (--tries > 0);
            if (tries == 0)
            {
                var UNRESOLVED = AddOngoing(0, StateCode.Error, $"Failed to apply changes for {Path.GetFileName(e.FullPath)} as file could not be opened");
                var timer = new System.Timers.Timer(5000);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    timer.Close();
                    timer.Dispose();
                    UNRESOLVED.Dispose();
                };
                timer.Start();
                return;
            }
            if (
              AppData.Files.ContainsKey(e.FullPath) &&
              checksum != AppData.Files[e.FullPath].MD5)
                GIO.QueueOperation(
                    () => gio.Update(e.FullPath),
                    AddOngoing(1, StateCode.Pending, $"Applying changes to {Path.GetFileName(e.FullPath)}")
                );
        }
        public static StateNotifcation AddOngoing(int folderIndex, StateCode state, string msg)
        {
            var cancel = new StateNotifcation
            {
                CancelToken = DateTime.Now.Ticks,
                Folder = folderIndex
            };
            Ongoing[cancel.Folder][cancel.CancelToken] = new StateInfo
            {
                State = state,
                Message = msg
            };
            StateChanged(cancel);
            return cancel;
        }
        public static void OpenLocal(object sender, EventArgs e)
        {
            if (AppData.Path != null) System.Diagnostics.Process.Start("explorer.exe", AppData.Path);
        }

        public static void OpenDrive(object sender, EventArgs e)
        {
            string folderID;
            if (gio.GetSettings().TryGetValue("folder", out folderID))
                System.Diagnostics.Process.Start($"https://drive.google.com/drive/u/0/folders/{folderID}");
        }
        public static void ClearDrive(bool showClearing = false)
        {
            GIO.QueueOperation(() =>
            {
                gio.UpdateFolder(null);
                AppData.Files = null;
                Watcher.EnableRaisingEvents = false;
                AppData.PageToken = null;
                AppData.Save();
                Drive.CancelToken = AddOngoing(1, StateCode.Error, "Select a Google Drive folder");
            },
            showClearing ? AddOngoing(1, StateCode.Pending, "Clearing Google Drive folder") : (StateNotifcation?)null);
        }
        public static void ClearLocal(bool showClearing = false, StateCode state = StateCode.Error, string msg = "Select a local folder")
        {
            GIO.QueueOperation(() =>
            {
                AppData.Path = null;
                AppData.Files = null;
                Watcher.EnableRaisingEvents = false;
                AppData.PageToken = null;
                AppData.Save();
                Local.CancelToken = AddOngoing(0, state, msg);
            },
            showClearing ? AddOngoing(0, StateCode.Pending, "Clearing Local Drive folder") : (StateNotifcation?)null);
        }
    }
    public struct StateNotifcation : IDisposable
    {
        public long CancelToken;
        public int Folder;
        public void Dispose()
        {
            Program.Ongoing[Folder].Remove(CancelToken);
            Program.StateChanged(this);
        }
    }
    struct StateInfo
    {
        public string Message;
        public StateCode State;
    }
}
