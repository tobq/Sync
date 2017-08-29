using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sync
{
    public partial class Canvas : Form
    // abstract public partial class Canvas : Form
    {
        public static NotifyIcon Tray = new NotifyIcon();
        dragger Dragger = new dragger();
        protected Pen border = new Pen(Program.Colours.text, 2);
        static ToolStripMenuItem settings = new ToolStripMenuItem("Settings", null, OpenSettings);
        static bool hidden = false;

        public Canvas()
        {
            InitializeComponent();
            Tray.ContextMenuStrip = TrayMenu;
            Tray.DoubleClick += OpenSettings;
        }
        static public void SetTrayOpen()
        {
            ((ToolStripMenuItem)Tray.ContextMenuStrip.Items[0]).DropDownItems[0].Enabled = AppData.Path == null ? false : true;
            ((ToolStripMenuItem)Tray.ContextMenuStrip.Items[0]).DropDownItems[1].Enabled = Program.gio.GetSettings().ContainsKey("folder");
        }
        public static void OpenSettings(object sender = null, EventArgs e = null)
        {
            if (hidden)
            {
                Program.Settings.show();
            }
            Program.Settings.WindowState = FormWindowState.Normal;
            Program.Settings.TopMost = true;
            Program.Settings.TopMost = false;
        }

        static public void UpdateTrayIcon(StateNotification? cancel = null)
        {
            if (Program.Ongoing[0].Count + Program.Ongoing[1].Count == 0) Tray.Icon = Program.StateIcons[(int)StateCode.OK];
            else if (Program.Ongoing[0].Count > 0 && Program.Ongoing[0].Count > 0)
            {
                Tray.Icon = Program.StateIcons[Math.Min(
                    (int)(Program.Ongoing[0].Count > 0 ? Program.Ongoing[0].Min(x => x.Value.State) : StateCode.OK),
                    (int)(Program.Ongoing[1].Count > 0 ? Program.Ongoing[1].Min(x => x.Value.State) : StateCode.OK)
                )];
            }
            else try
                {
                    Tray.Icon = Program.StateIcons[(int)Program.Ongoing[Program.Ongoing[0].Count == 0 ? 1 : 0].Min(x => x.Value.State)];
                }
                catch
                {
                    Tray.Icon = Program.StateIcons[(int)StateCode.OK];
                }
            Tray.Visible = true;
        }
        void DragMove(object sender, MouseEventArgs e) { if (Dragger.Down) Location = new Point(Location.X + e.X - Dragger.X, Location.Y + e.Y - Dragger.Y); }
        protected virtual void hide(object sender = null, EventArgs e = null)
        {
            if (Program.gio.Logged)
            {
                Hide();
                hidden = true;
                Tray.ContextMenuStrip.Items.Insert(1, settings);
            }
            else Close();
        }
        void MinClick(object sender, EventArgs e) { WindowState = FormWindowState.Minimized; }
        void onMouseUp(object sender, MouseEventArgs e) { Dragger.Down = false; }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(border, border.Width / 2, border.Width / 2, ClientSize.Width - border.Width, ClientSize.Height - border.Width);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                hide();
            }
            base.OnFormClosing(e);
        }
        void DragDown(object sender, MouseEventArgs e)
        {
            Dragger.Down = true;
            Dragger.X = e.X;
            Dragger.Y = e.Y;
        }
        public struct dragger
        {
            public bool Down;
            public int X, Y;
        }
        protected virtual void NextClick(object sender, EventArgs e) { }
        public virtual void show(Form opener = null)
        {
            hidden = false;
            if (opener == null) Show();
            else
            {
                Show();
                Location = opener.Location;
                Invalidate();
                Update();
                opener.Hide();
            }
            WindowState = FormWindowState.Normal;
            BringToFront();
            Tray.ContextMenuStrip.Items.Remove(settings);
        }
        //public abstract void NextClick(object sender, EventArgs e);
    }

}
