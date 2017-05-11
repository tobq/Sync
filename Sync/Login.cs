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
    public partial class Login : Canvas
    {
        public Login()
        {
            InitializeComponent();
            Next.Text = "Retry";
            Next.Visible = false;
        }
        public override void show(Form opener = null)
        {
            Utils.SetBrowser();
            Browser.Navigate(Program.gio.OAuth);
            base.show(opener);
        }
        protected override void NextClick(object sender, EventArgs e)
        {
            ErrMessage.Visible = Err.Visible = Error.Visible = Next.Visible = false;
            Browser.Navigate(Program.gio.OAuth);
        }
        protected override void hide(object sender = null, EventArgs e = null)
        {
            Program.Close();
        }
        void BrowserLoad(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var Title = Browser.DocumentTitle;
            var title = Title.ToLower();
            Browser.Visible = false;
            if (title.IndexOf("success code=") != -1)
            {
                Program.gio.Login(Title.Substring(13));
                Program.Settings.SetImage();
                Program.Settings.show(this);
            }
            else if (title.IndexOf("error=") != -1)
            {
                ErrMessage.Text = Title;
                ErrMessage.Visible = Err.Visible = Error.Visible = Next.Visible = true;
            }
            else Browser.Visible = true;
        }
    }
}
