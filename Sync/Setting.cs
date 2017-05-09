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
    public partial class Setting : Canvas
    //abstract public partial class Setting : Canvas
    {
        public Setting()
        {
            InitializeComponent();
            Next.Text = "Save";
        }
        protected virtual void BackClick(object sender, EventArgs e) { Program.Settings.show(this); }
        protected void save(Form sender)
        {
            Program.Settings.show(sender);
            AppData.Save();
        }
        //protected override abstract void NextClick(object sender, EventArgs e);
    }
}
