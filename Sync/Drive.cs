using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sync
{
    public partial class Drive : Setting
    {
        StateNotification? cancelToken;
        public StateNotification? CancelToken
        {
            set
            {
                if (cancelToken.HasValue) cancelToken.Value.Dispose();
                else Program.StateChanged();
                cancelToken = value;
            }
        }
        public static Control lastVisited;
        public Drive()
        {
            InitializeComponent();
            Title.Text = "Select a Google Drive folder";
        }
        public void Visit(string parent, string name, bool append = true)
        {
            Folders.SuspendLayout();
            Control newPath;
            var FolderControls = new Control[Folders.Controls.Count];
            var newFolders = Program.gio.FolderList(parent);
            var newFolderControls = new Control[newFolders.Count];
            for (var i = 0; i < newFolders.Count; i++) newFolderControls[i] = new DriveFolder(newFolders[i]);
            Folders.Controls.CopyTo(FolderControls, 0);
            Folders.Controls.Clear();
            Folders.Controls.AddRange(newFolderControls);
            foreach (Control control in FolderControls) control.Dispose();
            if (append)
            {
                newPath = new DrivePath(name, parent);
                if (Path.Controls.Count > 0) Path.Controls.Add(new Arrow());
                Path.Controls.Add(newPath);
                Path.ScrollControlIntoView(newPath);
            }
            else newPath = Path.Controls.Find(parent, false)[0];
            NewLast(newPath, null);
            Folders.ResumeLayout();
        }
        public static void NewLast(object sender, EventArgs e = null)
        {
            var Folder = ((Control)sender);
            if (lastVisited != null)
            {
                lastVisited.BackColor = Color.Transparent;
                lastVisited.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            }
            lastVisited = Folder;
            lastVisited.BackColor = Color.FromArgb(40, 0, 0, 0);
            lastVisited.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
            Grad.GradUpdate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(border, new Point(50, 158), new Point(400, 158));
        }
        protected override void NextClick(object sender, EventArgs e)
        {
            string folderID;
            if (Program.gio.GetSettings().TryGetValue("folder", out folderID) && lastVisited.Name == folderID)
            {
                Program.Settings.show(this);
                CancelToken = null;
                return;
            }
            if (AppData.Path == null)
            {
                Program.gio.UpdateFolder(lastVisited.Name);
                Program.Settings.show(this);
                CancelToken = null;
                clearFolders();
                resetText();
            }
            else
            {
                var localFilled = Directory.Exists(AppData.Path) && Directory.EnumerateFileSystemEntries(AppData.Path).Any();
                if (localFilled && Program.gio.HasChildren(lastVisited.Name))
                {

                    Info.Text = "At least one of your selected folders must be empty for set up. Select an empty folder, or change your local folder";
                    Info.ForeColor = Color.FromArgb(150, 0, 0);
                    return;
                }
                Program.Watcher.EnableRaisingEvents = false;
                Program.gio.UpdateFolder(lastVisited.Name);
                AppData.Files = new Dictionary<string, _File>();

                if (localFilled) GIO.QueueOperation(
                    () =>
                    {
                        Program.gio.FillRemote(lastVisited.Name,AppData.Path);
                        if (!string.IsNullOrEmpty(Program.Watcher.Path)) Program.Watcher.EnableRaisingEvents = true;
                    },
                    Program.AddOngoing(1, StateCode.Pending, "Setting up Google Drive folder")
                );
                else
                {
                    GIO.QueueOperation(
                        () =>
                        {
                            Program.gio.FillLocal(AppData.Path,lastVisited.Name);
                            Program.Watcher.EnableRaisingEvents = true;
                        },
                        Program.AddOngoing(0, StateCode.Pending, "Setting up local folder")
                    );
                    Program.Watcher.Path = AppData.Path;
                }
                Program.Settings.show(this);
                CancelToken = null;
                clearFolders();
                resetText();
            }
        }
        public override void show(Form opener)
        {
            Program.Drive.Visit("root", "Drive");
            base.show(opener);
        }
        void resetText()
        {
            Info.Text = "This folder will be automatically synchronised with your Google Drive";
            Info.ForeColor = Color.Black;
        }
        protected override void BackClick(object sender, EventArgs e)
        {
            base.BackClick(sender, e);
            Info.Text = "This Google Drive folder will be automatically synchronised with your local folder";
            Info.ForeColor = Color.Black;
            clearFolders();
        }
        void clearFolders()
        {
            var FolderControls = new Control[Folders.Controls.Count];
            var PathControls = new Control[Path.Controls.Count];
            Folders.Controls.CopyTo(FolderControls, 0);
            Path.Controls.CopyTo(PathControls, 0);
            Folders.Controls.Clear();
            Path.Controls.Clear();
            foreach (Control control in FolderControls) control.Dispose();
            foreach (Control control in PathControls) control.Dispose();
        }
    }
    public class Fileder : Label
    {
        Image icon;
        public Fileder()
        {
            Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            Margin = new Padding(0);
            TextAlign = ContentAlignment.MiddleLeft;
        }

        protected Image Icon
        {
            set
            {
                if (value != null)
                {
                    Padding = new Padding(value.Width + 15, 0, 0, 0);
                    icon = value;
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(icon, 10, (Height - icon.Height) / 2);
        }
        protected Image DriveIcon(string mime, int iconSize = 32)
        {
            return Utils.GetImage($"https://drive-thirdparty.googleusercontent.com/{iconSize}/type/{mime}");
        }
    }
    class DriveFolder : Fileder
    {
        public DriveFolder(Google.Apis.Drive.v3.Data.File folder, int iconSize = 32)
        {
            if (folder.Shared == true) folder.MimeType += "+shared";
            Icon = DriveIcon(folder.MimeType);
            Name = folder.Id;
            Text = folder.Name;
            Size = new Size(350, 50);
            Click += Drive.NewLast;
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            Program.Drive.Visit(Name, Text);
        }
    }
    public class DrivePath : Label
    {
        static void PathClick(object sender, EventArgs e)
        {
            var path = ((DrivePath)sender);
            if (path != Drive.lastVisited)
            {
                Program.Drive.Visit(path.Name, path.Text, false);
                var j = Program.Drive.Path.Controls.IndexOfKey(path.Name);
                for (var i = Program.Drive.Path.Controls.Count; --i > j;)
                {
                    var control = Program.Drive.Path.Controls[i];
                    Program.Drive.Path.Controls.Remove(control);
                    control.Dispose();
                }
                Grad.GradUpdate();
            }
        }
        public DrivePath(string name, string ID)
        {
            Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            Name = ID;
            Text = name;
            Padding = new Padding(15, 0, 15, 0);
            Margin = new Padding(0);
            BackColor = Color.Transparent;
            MinimumSize = new Size(0, 50);
            AutoSize = true;
            TextAlign = ContentAlignment.MiddleCenter;
            Click += PathClick;
        }
    }
    public class Arrow : Label
    {
        public Arrow()
        {

            Name = "arrow";
            Padding = new Padding(0);
            Margin = new Padding(0, 0, 15, 0);
            BackColor = Color.Transparent;
            Image = Properties.Resources.arrow;
            Size = new Size(8, 50);
        }
    }
    public class Grad : Label
    {
        public Grad()
        {
            Location = new Point(50, 107);
            Margin = new Padding(0);
            Name = "Grad";
            Size = new Size(65, 50);
        }
        public static void GradUpdate(object sender = null, MouseEventArgs e = null)
        {
            Program.Drive.Grad.Invalidate();
            Program.Drive.Grad.Update();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var bm = new Bitmap(Width, Height))
            {
                Program.Drive.Path.DrawToBitmap(bm, Program.Drive.Path.ClientRectangle);
                e.Graphics.DrawImage(bm, ClientRectangle);
            }
            if (Program.Drive.Path.HorizontalScroll.Value > 0)
                e.Graphics.FillRectangle(new LinearGradientBrush(ClientRectangle, Color.White, Color.Transparent, 0f), ClientRectangle);
        }
        protected override void WndProc(ref Message message)
        {
            const int Click = 0x0084;
            const int ClickThrough = (-1);
            if (message.Msg == Click) message.Result = (IntPtr)ClickThrough;
            else base.WndProc(ref message);
        }
    }
}
