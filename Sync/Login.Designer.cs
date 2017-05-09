namespace Sync
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.Err = new System.Windows.Forms.Label();
            this.Error = new System.Windows.Forms.Label();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Next.FlatAppearance.BorderSize = 0;
            this.Next.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.Location = new System.Drawing.Point(60, 526);
            this.Next.Click += new System.EventHandler(this.NextClick);
            // 
            // Err
            // 
            this.Err.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Err.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Err.Image = ((System.Drawing.Image)(resources.GetObject("Err.Image")));
            this.Err.Location = new System.Drawing.Point(82, 242);
            this.Err.Name = "Err";
            this.Err.Size = new System.Drawing.Size(48, 48);
            this.Err.TabIndex = 5;
            this.Err.Visible = false;
            // 
            // Error
            // 
            this.Error.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Error.Location = new System.Drawing.Point(151, 224);
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(256, 83);
            this.Error.TabIndex = 6;
            this.Error.Text = "An error occured while trying to authorise the application.";
            this.Error.Visible = false;
            // 
            // ErrMessage
            // 
            this.ErrMessage.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ErrMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ErrMessage.Location = new System.Drawing.Point(83, 325);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(293, 71);
            this.ErrMessage.TabIndex = 6;
            this.ErrMessage.Visible = false;
            // 
            // Browser
            // 
            this.Browser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Browser.Location = new System.Drawing.Point(0, 37);
            this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(470, 593);
            this.Browser.TabIndex = 3;
            this.Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.BrowserLoad);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(470, 630);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.Error);
            this.Controls.Add(this.Err);
            this.Controls.Add(this.Browser);
            this.Name = "Login";
            this.Controls.SetChildIndex(this.Next, 0);
            this.Controls.SetChildIndex(this.Browser, 0);
            this.Controls.SetChildIndex(this.Err, 0);
            this.Controls.SetChildIndex(this.Error, 0);
            this.Controls.SetChildIndex(this.ErrMessage, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.Label Err;
        private System.Windows.Forms.Label Error;
        private System.Windows.Forms.Label ErrMessage;
    }
}

