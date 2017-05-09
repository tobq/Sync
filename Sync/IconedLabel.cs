using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace Sync
{
    public class IconedLabel : Label
    {
        void setPadding()
        {
            if (icon == null) Padding = new Padding(0);
            else if (IconAlign == HorizontalAlignment.Right) Padding = new Padding(0, 0, (int)Math.Round(icon.Width * 1.2), 0);
            else Padding = new Padding((int)Math.Round(icon.Width * 1.2), 0, 0, 0);
        }
        HorizontalAlignment iconalign = HorizontalAlignment.Right;
        [Category("Appearance")]
        [DisplayName("IconAlign")]
        public HorizontalAlignment IconAlign
        {
            get { return iconalign; }
            set
            {
                iconalign = value;
                setPadding();
            }
        }

        protected Image icon;
        [Category("Appearance")]
        [DisplayName("Icon")]
        public Image Icon
        {
            get { return icon; }
            set
            {
                if (value != null)
                {
                    icon = value;
                    setPadding();
                    MinimumSize = new Size(0, icon.Height);
                }
            }
        }
        public IconedLabel()
        {
            AutoSize = true;
            BackColor = Color.Transparent;
            Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            ForeColor = Program.Colours.text;
            Location = new Point(50, 259);
            Size = new Size(57, 18);
            TextAlign = ContentAlignment.MiddleLeft;
            TabIndex = 6;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (icon != null)
            {
                if (IconAlign == HorizontalAlignment.Right)
                    e.Graphics.DrawImage(icon, Width - icon.Width, (Height - icon.Height) / 2, icon.Height, icon.Width);
                else
                    e.Graphics.DrawImage(icon, 0, (Height - icon.Height) / 2, icon.Height, icon.Width);
            }
        }
    }
    public class InfoLabel : IconedLabel
    {
        public InfoLabel()
        {
            icon = Properties.Resources.info_m;
            IconAlign = HorizontalAlignment.Left;
            Font = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.Pixel);
            ForeColor = Color.Black;
            AutoSize = false;
        }
    }
}

