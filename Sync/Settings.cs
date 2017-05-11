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
    public partial class Settings : Canvas
    {
        protected override void NextClick(object sender, EventArgs e) { hide(); }
        Color[] folderColours = new Color[] { Program.Colours.text, Program.Colours.text };
        ToolTip MouseTip = new ToolTip();
        public Settings()
        {
            InitializeComponent();
            Next.Text = "Done";
            onstatechange = new Action<StateNotification?>(delegate (StateNotification? cancel)
            {
                if (cancel.HasValue)
                {
                    if (Program.Ongoing[cancel.Value.Folder].Count == 0)
                    {
                        if (cancel.Value.Folder == 0)
                        {
                            Local.Text = Path.GetFileName(AppData.Path);
                            ClearL.Visible = true;
                            Local.Padding = new Padding(105, 30, 100, 0);
                        }
                        else
                        {
                        string folderID;
                            if (Program.gio.GetSettings().TryGetValue("folder", out folderID)) Drive.Text = Program.gio.IDToName(folderID);
                            ClearD.Visible = true;
                            Drive.Padding = new Padding(105, 30, 100, 0);
                        }
                        folderColours[cancel.Value.Folder] = Program.StateColours[(int)StateCode.OK];
                    }
                    else
                    {
                        var eventinfo = Program.Ongoing[cancel.Value.Folder].OrderBy(e => e.Value.State).ThenByDescending(e => e.Key).First();
                        folderColours[cancel.Value.Folder] = Program.StateColours[(int)eventinfo.Value.State];
                        if (cancel.Value.Folder == 0)
                        {
                            Local.Text = eventinfo.Value.Message;
                            ClearL.Visible = false;
                            Local.Padding = new Padding(105, 30, 50, 0);
                        }
                        else
                        {
                            Drive.Text = eventinfo.Value.Message;
                            ClearD.Visible = false;
                            Drive.Padding = new Padding(105, 30, 50, 0);
                        }
                    }
                    if (GIO.QuietTime || Program.Ongoing[0].Count + Program.Ongoing[1].Count == 0)
                    {
                        MouseTip.RemoveAll();
                        Advanced.ForeColor = Color.Black;
                    }
                    else
                    {
                        MouseTip.SetToolTip(Advanced, "You must finish the folder setup and wait for operations\n to finish to access the advanced options menu");
                        Advanced.ForeColor = Color.FromArgb(180, 180, 180);
                    }
                    Invalidate();
                    Update();
                }
            });
        }
        public void SetImage()
        {
            Profile.Image = GIO.user.getPhoto(Profile.Width, true);
        }
        public override void show(Form opener = null)
        {
            SetTrayOpen();
            Program.gio.SetQuietHours();
            Email.Text = Utils.Cap(GIO.user.email);
            base.show(opener);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.DrawLine(border, new Point(50, 275), new Point(400, 275));
            g.DrawLine(border, new Point(50, 387), new Point(400, 387));
            using (var b = new SolidBrush(Program.Colours.grey))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.High;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.FillEllipse(b, 65, 220, 30, 30);
                g.FillEllipse(b, 65, 330, 30, 30);
                b.Color = folderColours[0];
                g.FillEllipse(b, 70, 225, 20, 20);
                b.Color = folderColours[1];
                g.FillEllipse(b, 70, 335, 20, 20);
            }
        }
        public void OnStateChanged(StateNotification? cancel)
        {
            if (InvokeRequired) Invoke(onstatechange, cancel);
            else onstatechange(cancel);
        }
        Action<StateNotification?> onstatechange;
        void LogoutClick(object sender, EventArgs e)
        {
            GIO.Logout(this);
        }
        private void LocalClick(object sender, EventArgs e)
        {
            Program.Local.show(this);
        }
        private void DriveClick(object sender, EventArgs e)
        {
            Program.Drive.show(this);
        }

        private void AdvancedClick(object sender, EventArgs e)
        {
            Program.Advanced.show(this);
        }

        public class ClearFolder : Label
        {
            public ClearFolder()
            {
                AutoSize = true;
                FlatStyle = FlatStyle.Flat;
                Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
                BackColor = Color.Transparent;
                ForeColor = Color.FromArgb(50, 50, 50);
                Location = new Point(338, 317);
                Margin = new Padding(0);
                Name = "Clear";
                Text = "Clear";
            }
        }
        void clearDClick(object sender = null, EventArgs e = null)
        {
            Program.ClearDrive(true);
        }
        void clearLClick(object sender, EventArgs e)
        {
            Program.ClearLocal(true);
        }
    }
}
