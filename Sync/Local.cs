using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sync
{
    public partial class Local : Setting
    {
        StateNotification? cancelToken;
        public StateNotification? CancelToken
        {
            set
            {
                if (cancelToken.HasValue) cancelToken.Value.Dispose();
                cancelToken = value;
            }
        }
        public FolderBrowserDialog dialog = new FolderBrowserDialog();
        public Local()
        {
            InitializeComponent();
            Title.Text = dialog.Description = "Select your local folder";
            dialog.ShowNewFolderButton = true;
        }
        protected override void NextClick(object sender, EventArgs e)
        {
            if (Path.Text == AppData.Path)
            {
                Program.Settings.show(this);
                CancelToken = null;
                return;
            }
            if (Directory.Exists(Path.Text))
            {
                string folderID;
                if (!Program.gio.GetSettings().TryGetValue("folder", out folderID))
                {
                    AppData.Path = new DirectoryInfo(Path.Text).FullName;
                    Program.Settings.show(this);
                    CancelToken = null;
                    resetText();
                }
                else
                {
                    var localFilled = Directory.EnumerateFileSystemEntries(Path.Text).Any();
                    if (localFilled && Program.gio.HasChildren(folderID))
                    {
                        Info.Text = "At least one of your selected folders must be empty for set up. Select an empty folder, or change your Google Drive folder";
                        Info.ForeColor = Color.FromArgb(150, 0, 0);
                        return;
                    }
                    Program.Watcher.EnableRaisingEvents = false;
                    Program.Watcher.Path = AppData.Path = Path.Text;
                    AppData.Files = new Dictionary<string, _File>();

                    if (localFilled) GIO.AddOperation(
                        () =>
                        {
                            Program.gio.FillRemote(folderID, AppData.Path);
                            Program.Watcher.EnableRaisingEvents = true;
                        },
                        Program.AddOngoing(1, StateCode.Pending, "Setting up Google Drive folder"), null
                    );
                    else
                    {
                        GIO.AddOperation(
                            () =>
                            {
                                Program.gio.FillLocal(AppData.Path, folderID);
                                Program.Watcher.EnableRaisingEvents = true;
                            },
                            Program.AddOngoing(0, StateCode.Pending, "Setting up local folder"), null
                        );
                    }
                    Program.Settings.show(this);
                    CancelToken = null;
                    resetText();

                }
            }
            else
            {
                Info.Text = "That folder does not exist";
                Info.ForeColor = Color.FromArgb(150, 0, 0);
                return;
            }
        }
        void resetText()
        {
            Info.Text = "This folder will be automatically synchronised with your Google Drive";
            Info.ForeColor = Color.Black;
        }
        protected override void BackClick(object sender, EventArgs e)
        {
            base.BackClick(sender, e);
            resetText();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(border, new Point(50, 196), new Point(400, 196));
        }

        public override void show(Form opener)
        {
            if (Program.Ongoing[0].Values.Any(og => og.State == StateCode.Pending)) return;
            Path.Text = AppData.Path;
            ActiveControl = Path;
            base.show(opener);
        }

        private void PanelClick(object sender, EventArgs e) { ActiveControl = Path; }

        private void BrowseClick(object sender, EventArgs e)
        {
            if (dialog.ShowDialog(this) == DialogResult.OK) Path.Text = dialog.SelectedPath;
        }

        private void PathTextChanged(object sender, EventArgs e)
        {
            Folder.Text = Path.Text == null || Path.Text.Length < 1 ?
                Folder.Text = "No folder selected" : new DirectoryInfo(Path.Text).Name;
        }
    }
}
