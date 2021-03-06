﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

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


        public delegate void StateChange(StateNotification? cancel = null);
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
        static Action closeAction = () =>
        {
            Canvas.Tray.Dispose();
            if (gio.Logged) AppData.Save();
            Application.Exit();
            Environment.Exit(0);
        };
        public static void Close(object sender = null, EventArgs e = null)
        {
            if (GIO.QuietTime) closeAction();
            else GIO.AddOperation(closeAction, AddOngoing(0, StateCode.Misc, "Closing Application..."), null);
        }

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
                if (created)
                {
                    var singleInstanceThread = new Thread(() =>
                    {
                        while (true)
                        {
                            hevent.WaitOne();
                            if (gio.Logged) Settings.Invoke((MethodInvoker)delegate { Canvas.OpenSettings(); });
                            else Login.Invoke((Action)(() => Login.show()));
                            hevent.Reset();
                        }
                    });
                    singleInstanceThread.SetApartmentState(ApartmentState.STA);
                    singleInstanceThread.Start();
                }
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
                else StateChanged(new StateNotification { Folder = 0 });
                if (gio.GetSettings().TryGetValue("folder", out folderID)) StateChanged(new StateNotification { Folder = 1 });
                else ClearDrive();
                AppData.Save();

                if (AppData.Files != null)
                {
                    if (Directory.Exists(AppData.Path))
                    {
                        Watcher.Path = AppData.Path;
                        Watcher.EnableRaisingEvents = true;
                        gio.PatchRemote();
                    }
                    else ClearDrive();
                }
            }
            else Login.show();

            Application.EnableVisualStyles();
            Application.Run();
        }

        public static void onDelete(object sender, FileSystemEventArgs e)
        {
            if (AppData.Files.ContainsKey(e.FullPath) && !AppData.Trashed.Contains(e.FullPath) && !RecentlyDeleted.Contains(e.FullPath))
            {
                RecentlyDeleted.Add(e.FullPath);
                var timer = new System.Timers.Timer(1000);
                timer.AutoReset = false;
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
                        GIO.AddOperation(
                            () =>
                            {
                                gio.Trash(trashID, true);
                                gio.UpdateProperties(trashID, "prioritised", "false");
                            },
                            AddOngoing(1, StateCode.Pending, $"Trashing {e.Name}"),
                            gio.GetPriority(trashID)
                        );
                    }
                };
                timer.Start();
            }
        }

        public static void onCreate(object s, FileSystemEventArgs e)
        {
            if (AppData.Files.ContainsKey(e.FullPath) && !AppData.Trashed.Contains(e.FullPath)) return;
            AppData.Trashed.Remove(e.FullPath);

            if (Directory.Exists(e.FullPath))
            {
                GIO.AddOperation(
                    () => AppData.Files[e.FullPath] = new _File
                    {
                        ID = gio.NewFolder(e.Name,
                        AppData.Files[Path.GetDirectoryName(e.FullPath)].ID)
                    },
                    AddOngoing(1, StateCode.Pending, $"Creating new folder {e.Name}"),
                    -1
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
                    var UNRESOLVED = AddOngoing(0, StateCode.Error, $"Failed to apply changes for {e.Name} as the file could not be opened");
                    return;
                }
                foreach (var deletedKey in RecentlyDeleted.ToArray())
                {
                    if (AppData.Files[deletedKey].MD5 == checksum
                        && Path.GetFileName(deletedKey) == e.Name)
                    {
                        RecentlyDeleted.Remove(deletedKey);
                        GIO.AddOperation(
                            () => gio.Move(deletedKey, e.FullPath),
                            AddOngoing(1, StateCode.Pending, $"Moving {e.Name}"),
                            gio.GetPriority(AppData.Files[deletedKey].ID)
                        );
                        return;
                    }
                }
                foreach (var trashedKey in AppData.Trashed.ToArray())
                {
                    if (AppData.Files.ContainsKey(trashedKey))
                    {
                        if (AppData.Files[trashedKey].MD5 == checksum
                            && Path.GetFileName(trashedKey) == e.Name)
                        {
                            AppData.Trashed.Remove(trashedKey);
                            var trashedID = AppData.Files[trashedKey].ID;
                            GIO.AddOperation(
                                () => gio.Trash(trashedID, false),
                                AddOngoing(1, StateCode.Pending, $"Untrashing {e.Name}"),
                                gio.GetPriority(trashedID)
                            );
                            return;
                        }
                    }
                    else AppData.Trashed.Remove(trashedKey);
                }
                GIO.AddOperation(
                    () => gio.Upload(e.FullPath, AppData.Files[Path.GetDirectoryName(e.FullPath)].ID),
                    AddOngoing(1, StateCode.Pending, $"Uploading {e.Name}"),
                    -1);
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
                var newName = e.Name;
                GIO.AddOperation(
                    () => gio.Rename(file.ID, newName),
                    AddOngoing(1, StateCode.Pending, $"Renaming remote item to {newName}"),
                    gio.GetPriority(file.ID)
                );
            }
            else
                GIO.AddOperation(
               () => gio.Upload(e.FullPath, AppData.Files[Path.GetDirectoryName(e.FullPath)].ID),
               AddOngoing(1, StateCode.Pending, $"Uploading {e.Name}"),
               -1);
        }

        public static void onChange(object s, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath)) return;
            if (AppData.Files.ContainsKey(e.FullPath))
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
                    var UNRESOLVED = AddOngoing(0, StateCode.Error, $"Failed to apply changes for {e.Name} as file could not be opened");
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
                if (checksum != AppData.Files[e.FullPath].MD5)
                    GIO.AddOperation(
                        () => gio.Update(e.FullPath),
                        AddOngoing(1, StateCode.Pending, $"Applying changes to {e.Name}"),
                        gio.GetPriority(AppData.Files[e.FullPath].ID)
                    );
            }
        }
        public static StateNotification AddOngoing(int folderIndex, StateCode state, string msg)
        {
            var cancel = new StateNotification
            {
                CancelToken = DateTime.Now.Ticks,
                Folder = folderIndex
            };
            Ongoing[cancel.Folder][cancel.CancelToken] = new StateInfo
            {
                State = state,
                Message = msg
            };
            StateChanged?.Invoke(cancel);
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
            GIO.AddOperation(() =>
            {
                gio.UpdateFolder(null);
                AppData.Files = null;
                Watcher.EnableRaisingEvents = false;
                AppData.PageToken = null;
                AppData.Save();
                Drive.CancelToken = AddOngoing(1, StateCode.Error, "Select a Google Drive folder");
            },
            showClearing ? AddOngoing(1, StateCode.Pending, "Clearing Google Drive folder") : (StateNotification?)null, null);
        }
        public static void ClearLocal(bool showClearing = false, StateCode state = StateCode.Error, string msg = "Select a local folder")
        {
            GIO.AddOperation(() =>
            {
                AppData.Path = null;
                AppData.Files = null;
                Watcher.EnableRaisingEvents = false;
                AppData.PageToken = null;
                AppData.Save();
                Local.CancelToken = AddOngoing(0, state, msg);
            },
            showClearing ? AddOngoing(0, StateCode.Pending, "Clearing Local Drive folder") : (StateNotification?)null, null);
        }
    }
    //public class OpQueue
    //{
    //    Operation[] queue;
    //    int start = 0;
    //    int end = 0;
    //    public int Count {
    //        get {
    //            return end - start + 1 + (start > end ? queue.Length : 0);
    //        }
    //    }
    //    public OpQueue(int size = 32)
    //    {
    //        queue = new Operation[size];
    //    }
    //    public void Enqueue(Operation op)
    //    {
    //        lock (queue)
    //        {
    //            if ((start - ((end > queue.Length - 2) ? 0 : end)) == 0)
    //            {
    //                    var copy = queue;
    //                    queue = new Operation[queue.Length * 2];

    //            }
    //        }
    //    }
    //    public Operation Dequeue() {
    //        if (start == end) throw new KeyNotFoundException();
    //        var retVal = queue[start];
    //        start = start > queue.Length - 2 ? 0 : start + 1;
    //        return retVal;
    //    }
    //}
    public struct StateNotification : IDisposable
    {
        public long CancelToken;
        public int Folder;
        public void Dispose()
        {
            Program.Ongoing[Folder].Remove(CancelToken);
            Program.StateChanged(this);
        }
        public int Priority;
    }
    struct StateInfo
    {
        public string Message;
        public StateCode State;
    }
}
