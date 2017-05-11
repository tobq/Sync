using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FolderBrowserDialogEx;
using System.Threading;

namespace Sync
{
    public partial class Advanced : Setting
    {
        FBDEx dialog = new FBDEx();
        public Advanced()
        {
            InitializeComponent();
            Title.Text = "Advanced Options";
            FromHLabel.Padding = FromMLabel.Padding = ToMLabel.Padding = ToHLabel.Padding = new Padding(0, 5, SystemInformation.VerticalScrollBarWidth, 0);
            dialog.Title = "Add a priority";
            dialog.ShowEditbox = false;
            dialog.ShowFiles = true;
        }
        private void newPrioritised(object sender, EventArgs e)
        {
            var result = dialog.ShowDialog(this);
            var selected = Utils.Normalise(dialog.SelectedPath);
            var local = Utils.Normalise(AppData.Path);

            if (result == DialogResult.OK &&
                selected.StartsWith(local) &&
                selected.Length > local.Length &&
                !prioritised.Controls.ContainsKey(selected))
                prioritised.Controls.Add(new Prioritised(dialog.SelectedPath));
        }
        class PriorityPanel : FlowLayoutPanel
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                if (Controls.Count < 1)
                {
                    var nomsg = "No items in priority list";
                    var nosize = e.Graphics.MeasureString(nomsg, Font);
                    e.Graphics.DrawString(nomsg, Font, new SolidBrush(Color.Black), (Width - nosize.Width) / 2, 10);
                }
            }

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keys)
        {
            if (Prioritised.Selected != null)
            {
                switch (keys)
                {
                    case Keys.Down:
                        prioritised.Controls.SetChildIndex(Prioritised.Selected,
                            Utils.Clamp(prioritised.Controls.IndexOf(Prioritised.Selected) + 1,
                            0, prioritised.Controls.Count - 1));
                        break;
                    case Keys.Up:
                        prioritised.Controls.SetChildIndex(Prioritised.Selected,
                            Utils.Clamp(prioritised.Controls.IndexOf(Prioritised.Selected) + -1, 0, prioritised.Controls.Count - 1));
                        break;
                }
                prioritised.ScrollControlIntoView(Prioritised.Selected);
            }
            return base.ProcessCmdKey(ref msg, keys);
        }

        public class Prioritised : Fileder
        {
            static public ToolTip MouseTip = new ToolTip();
            class Remove : Button
            {
                public Remove()
                {
                    Text = "Remove";
                    Font = new Font("Arial", 12);
                    ForeColor = Color.FromArgb(50, 0, 0, 0);
                    Padding = Margin = new Padding(0);
                    FlatStyle = FlatStyle.Flat;
                    FlatAppearance.BorderSize = 0;
                    Size = new Size(80, 30);
                    Visible = false;
                    FlatAppearance.MouseOverBackColor = FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 0, 0, 0);
                    BackColor = Color.Transparent;
                }
                protected override void OnClick(EventArgs e)
                {
                    MouseTip.SetToolTip(Parent, null);
                    Removed.Add(Parent.Name);
                    Parent.Parent.Controls.Remove(Parent);
                    Parent.Dispose();
                }
            }
            public Button remove = new Remove();
            public static List<string> Removed = new List<string>();
            static Prioritised selected;
            public static Prioritised Selected
            {
                get { return selected; }
                set
                {
                    if (selected != null)
                    {
                        selected.BackColor = Color.Transparent;
                        selected.Font = new Font("Arial", 16F, GraphicsUnit.Pixel);
                        selected.remove.Visible = false;
                        selected.Padding = new Padding(selected.Padding.Left, 0, 0, 0);
                    }
                    selected = value;
                    if (value != null)
                    {
                        selected.remove.Visible = true;
                        selected.BackColor = Color.FromArgb(40, 0, 0, 0);
                        selected.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
                        selected.Padding = new Padding(selected.Padding.Left, 0, selected.remove.Width + selected.Height - selected.remove.Height, 0);
                    }
                }
            }
            public Prioritised(Google.Apis.Drive.v3.Data.File file)
            {
                Name = file.Id;
                Icon = DriveIcon(file.MimeType);
                Text = Path.GetFileName(file.Name);
                Size = new Size(350 - SystemInformation.VerticalScrollBarWidth, 50);
                AllowDrop = true;
                AutoEllipsis = true;
                AutoSize = false;
                MouseTip.SetToolTip(this, AppData.Files.FirstOrDefault(f => f.Value.ID == file.Id).Key);

                Controls.Add(remove);
                remove.Location = new Point((Width - remove.Width) - (Height - remove.Height) / 2, (Height - remove.Height) / 2);
            }
            public Prioritised(string path, int iconSize = 32)
            {
                _File item;
                if (AppData.Files.TryGetValue(path, out item)) Name = item.ID;
                else throw new ArgumentException();

                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    Icon = DriveIcon(GIO.FolderMIME);
                    Text = new DirectoryInfo(path).Name;
                }
                else
                {
                    Icon = DriveIcon(MIME.GetMimeType(Path.GetExtension(path)));
                    Text = Path.GetFileName(path);
                }
                Size = new Size(350 - SystemInformation.VerticalScrollBarWidth, 50);
                AllowDrop = true;
                AutoEllipsis = true;
                AutoSize = false;
                MouseTip.SetToolTip(this, path);

                Controls.Add(remove);
                remove.Location = new Point((Width - remove.Width) - (Height - remove.Height) / 2, (Height - remove.Height) / 2);
            }
            protected override void OnMouseDown(MouseEventArgs e)
            {
                Selected = this;
                DoDragDrop(this, DragDropEffects.Move);
            }
            protected override void OnDragEnter(DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
                if (selected != null && selected != this)
                {
                    var index = Parent.Controls.GetChildIndex(selected);
                    Parent.Controls.SetChildIndex(Selected, Parent.Controls.GetChildIndex(this));
                    Parent.Controls.SetChildIndex(this, index);
                    ((FlowLayoutPanel)Parent).ScrollControlIntoView(selected);
                }
            }
        }
        protected override void NextClick(object sender, EventArgs e)
        {
            using (var cancel = Program.AddOngoing(1, StateCode.Pending, "Synchronising your settings"))
            {
                Program.Settings.show(this);
                foreach (var removed in Prioritised.Removed) Program.gio.UpdateProperties(removed, "prioritised", "false");
                Program.gio.UpdateProperties(AppData.SettingID, "quietHours", From.Text.Replace(":", "") + To.Text.Replace(":", ""));
                Control[] priorities;
                priorities = new Control[prioritised.Controls.Count];
                prioritised.Controls.CopyTo(priorities, 0);
                for (var i = priorities.Length; i-- > 0;)
                {
                    Program.gio.UpdateProperties(priorities[i].Name, "prioritised", "true");
                    Program.gio.UpdateProperties(priorities[i].Name, "priority", i.ToString());
                }
                Program.gio.SetQuietHours(From.Text.Replace(":", "") + To.Text.Replace(":", ""));
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(border, new Point(50, 225), new Point(400, 225));
        }
        private void ToggleFrom(object sender, EventArgs e)
        {
            FromPanel.Visible = FromPanel.Visible ? false : true;
        }
        private void ToggleTo(object sender, EventArgs e)
        {
            ToPanel.Visible = ToPanel.Visible ? false : true;
        }
        public override void show(Form opener)
        {
            if (GIO.QuietTime || AppData.Files != null && Program.Ongoing[0].Count + Program.Ongoing[1].Count == 0)
            {
                Control[] oldpriorities = new Control[prioritised.Controls.Count];
                prioritised.Controls.CopyTo(oldpriorities, 0);
                prioritised.Controls.Clear();
                foreach (var control in oldpriorities) control.Dispose();
                foreach (var item in Program.gio.GetPrioritised()) prioritised.Controls.Add(new Prioritised(item));

                dialog.RootFolder = AppData.Path;
                ToPanel.Visible = FromPanel.Visible = false;
                string quiethours;
                if (!Program.gio.GetSettings().TryGetValue("quietHours", out quiethours)) quiethours = "00000000";
                FromH.SelectedIndex = short.Parse(quiethours.Substring(0, 2));
                FromM.SelectedIndex = short.Parse(quiethours.Substring(2, 2));
                ToH.SelectedIndex = short.Parse(quiethours.Substring(4, 2));
                ToM.SelectedIndex = short.Parse(quiethours.Substring(6, 2));
                if (quiethours.Substring(0, 4) == quiethours.Substring(4, 4)) clearTimes();
                else
                {
                    setTimes();
                    From.Font = To.Font = new Font("Arial", 16F);
                }
                base.show(opener);
            }
        }
        private void ListClick(object sender, EventArgs e)
        {
            setTimes();
            if (From.Text == To.Text)
            {
                From.Font = To.Font = new Font("Arial", 16, FontStyle.Italic);
                From.ForeColor = To.ForeColor = Program.Colours.text;
            }
            else
            {
                From.Font = To.Font = new Font("Arial", 16);
                From.ForeColor = To.ForeColor = Color.Black;
            }
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ToPanel.Visible = FromPanel.Visible = false;
            if (From.Text == To.Text) clearTimes();
            Prioritised.Selected = null;
        }
        void setTimes()
        {
            From.Text = $"{FromH.SelectedItem ?? "00"}:{FromM.SelectedItem ?? "00"}";
            To.Text = $"{ToH.SelectedItem ?? "00"}:{ToM.SelectedItem ?? "00"}";
        }
        void clearTimes(object sender = null, EventArgs e = null)
        {
            FromH.ClearSelected();
            FromM.ClearSelected();
            ToH.ClearSelected();
            ToM.ClearSelected();
            From.Text = "00:00";
            To.Text = "00:00";
            From.Font = To.Font = new Font("Arial", 16F, FontStyle.Italic);
            From.ForeColor = To.ForeColor = Program.Colours.text;
        }
    }


    class TimeList : ListBox
    {
        public TimeList()
        {
        }
        public TimeList(int length)
        {
            Items.AddRange((from i in Enumerable.Range(0, length) select i.ToString().PadLeft(2, '0')).ToArray());
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var item = Items[e.Index].ToString();
            var itemSize = e.Graphics.MeasureString(item, e.Font, 0);
            var centrePoint = new PointF(e.Bounds.Left + ((e.Bounds.Width - itemSize.Width) / 2), e.Bounds.Top + (e.Bounds.Height - itemSize.Height) / 2);
            e.Graphics.FillRectangle(new SolidBrush((e.State & DrawItemState.Selected) == DrawItemState.Selected ?
                Color.FromArgb(120, 170, 230) : Color.White), e.Bounds);
            e.Graphics.DrawString(item, e.Font, new SolidBrush(Color.Black), centrePoint);
        }
    }
    class SettingTitle : IconedLabel
    {
        protected static ToolTip MouseTip = new ToolTip();
        public SettingTitle()
        {
            icon = Properties.Resources.Info;
            Font = new Font("Arial", 16F, FontStyle.Bold);
        }
        string tip;
        [Category("Appearance")]
        [DisplayName("InfoTip")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", "System.Drawing.Design.UITypeEditor")]
        public string Tip
        {
            get { return tip; }
            set
            {
                MouseTip.SetToolTip(this, tip = value);
            }
        }
    }
}