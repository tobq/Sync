namespace Sync
{
    partial class Settings
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
            this.Profile = new System.Windows.Forms.PictureBox();
            this.Email = new System.Windows.Forms.Label();
            this.Logout = new System.Windows.Forms.Button();
            this.Drive = new System.Windows.Forms.Button();
            this.DriveLabel = new System.Windows.Forms.Label();
            this.ClearD = new Sync.IconedLabel();
            this.Local = new System.Windows.Forms.Button();
            this.LocalLabel = new System.Windows.Forms.Label();
            this.ClearL = new Sync.IconedLabel();
            this.Advanced = new System.Windows.Forms.Button();
            this.Home = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).BeginInit();
            this.Drive.SuspendLayout();
            this.Local.SuspendLayout();
            this.Home.SuspendLayout();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Next.FlatAppearance.BorderSize = 0;
            this.Next.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            // 
            // Profile
            // 
            this.Profile.BackColor = System.Drawing.Color.Transparent;
            this.Profile.Location = new System.Drawing.Point(59, 24);
            this.Profile.Name = "Profile";
            this.Profile.Size = new System.Drawing.Size(70, 70);
            this.Profile.TabIndex = 7;
            this.Profile.TabStop = false;
            // 
            // Email
            // 
            this.Email.BackColor = System.Drawing.Color.Transparent;
            this.Email.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Email.Location = new System.Drawing.Point(143, 24);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(238, 40);
            this.Email.TabIndex = 8;
            this.Email.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Logout
            // 
            this.Logout.BackColor = System.Drawing.Color.Transparent;
            this.Logout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Logout.FlatAppearance.BorderSize = 0;
            this.Logout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Logout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Logout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Logout.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Logout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.Logout.Location = new System.Drawing.Point(143, 59);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(70, 30);
            this.Logout.TabIndex = 9;
            this.Logout.Text = "Log out";
            this.Logout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Logout.UseVisualStyleBackColor = false;
            this.Logout.Click += new System.EventHandler(this.LogoutClick);
            // 
            // Drive
            // 
            this.Drive.AutoEllipsis = true;
            this.Drive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Drive.Controls.Add(this.DriveLabel);
            this.Drive.Controls.Add(this.ClearD);
            this.Drive.FlatAppearance.BorderSize = 0;
            this.Drive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Drive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Drive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Drive.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Drive.Location = new System.Drawing.Point(2, 276);
            this.Drive.Margin = new System.Windows.Forms.Padding(0);
            this.Drive.Name = "Drive";
            this.Drive.Padding = new System.Windows.Forms.Padding(105, 30, 100, 0);
            this.Drive.Size = new System.Drawing.Size(446, 111);
            this.Drive.TabIndex = 10;
            this.Drive.Text = "Loading...";
            this.Drive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Drive.UseVisualStyleBackColor = false;
            this.Drive.Click += new System.EventHandler(this.DriveClick);
            // 
            // DriveLabel
            // 
            this.DriveLabel.AutoSize = true;
            this.DriveLabel.BackColor = System.Drawing.Color.Transparent;
            this.DriveLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DriveLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.DriveLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.DriveLabel.Location = new System.Drawing.Point(50, 15);
            this.DriveLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DriveLabel.Name = "DriveLabel";
            this.DriveLabel.Size = new System.Drawing.Size(86, 16);
            this.DriveLabel.TabIndex = 11;
            this.DriveLabel.Text = "Drive Folder";
            this.DriveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DriveLabel.Click += Program.OpenDrive;
            // 
            // ClearD
            // 
            this.ClearD.AutoSize = true;
            this.ClearD.BackColor = System.Drawing.Color.Transparent;
            this.ClearD.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ClearD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClearD.Icon = global::Sync.Properties.Resources.Close;
            this.ClearD.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ClearD.Location = new System.Drawing.Point(370, 42);
            this.ClearD.MinimumSize = new System.Drawing.Size(0, 24);
            this.ClearD.Name = "ClearD";
            this.ClearD.Padding = new System.Windows.Forms.Padding(0, 0, 29, 0);
            this.ClearD.Size = new System.Drawing.Size(29, 24);
            this.ClearD.TabIndex = 6;
            this.ClearD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearD.Click += new System.EventHandler(this.clearDClick);
            // 
            // Local
            // 
            this.Local.AutoEllipsis = true;
            this.Local.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Local.Controls.Add(this.LocalLabel);
            this.Local.Controls.Add(this.ClearL);
            this.Local.FlatAppearance.BorderSize = 0;
            this.Local.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Local.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Local.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Local.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Local.Location = new System.Drawing.Point(2, 165);
            this.Local.Margin = new System.Windows.Forms.Padding(0);
            this.Local.Name = "Local";
            this.Local.Padding = new System.Windows.Forms.Padding(105, 30, 100, 0);
            this.Local.Size = new System.Drawing.Size(446, 111);
            this.Local.TabIndex = 10;
            this.Local.Text = "Loading...";
            this.Local.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Local.UseVisualStyleBackColor = false;
            this.Local.Click += new System.EventHandler(this.LocalClick);
            // 
            // LocalLabel
            // 
            this.LocalLabel.AutoSize = true;
            this.LocalLabel.BackColor = System.Drawing.Color.Transparent;
            this.LocalLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LocalLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.LocalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.LocalLabel.Location = new System.Drawing.Point(50, 15);
            this.LocalLabel.Margin = new System.Windows.Forms.Padding(0);
            this.LocalLabel.Name = "LocalLabel";
            this.LocalLabel.Size = new System.Drawing.Size(87, 16);
            this.LocalLabel.TabIndex = 11;
            this.LocalLabel.Text = "Local Folder";
            this.LocalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LocalLabel.Click += Program.OpenLocal;
            // 
            // ClearL
            // 
            this.ClearL.AutoSize = true;
            this.ClearL.BackColor = System.Drawing.Color.Transparent;
            this.ClearL.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ClearL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClearL.Icon = global::Sync.Properties.Resources.Close;
            this.ClearL.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ClearL.Location = new System.Drawing.Point(370, 42);
            this.ClearL.MinimumSize = new System.Drawing.Size(0, 24);
            this.ClearL.Name = "ClearL";
            this.ClearL.Padding = new System.Windows.Forms.Padding(0, 0, 29, 0);
            this.ClearL.Size = new System.Drawing.Size(29, 24);
            this.ClearL.TabIndex = 6;
            this.ClearL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearL.Click += new System.EventHandler(this.clearLClick);
            // 
            // Advanced
            // 
            this.Advanced.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Advanced.FlatAppearance.BorderSize = 0;
            this.Advanced.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Advanced.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Advanced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Advanced.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Advanced.Location = new System.Drawing.Point(2, 387);
            this.Advanced.Name = "Advanced";
            this.Advanced.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.Advanced.Size = new System.Drawing.Size(446, 78);
            this.Advanced.TabIndex = 10;
            this.Advanced.Text = "Advanced options";
            this.Advanced.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Advanced.UseVisualStyleBackColor = false;
            this.Advanced.Click += new System.EventHandler(this.AdvancedClick);
            // 
            // Home
            // 
            this.Home.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Home.Controls.Add(this.Profile);
            this.Home.Controls.Add(this.Logout);
            this.Home.Controls.Add(this.Email);
            this.Home.Location = new System.Drawing.Point(2, 35);
            this.Home.Margin = new System.Windows.Forms.Padding(0);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(446, 130);
            this.Home.TabIndex = 11;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(450, 560);
            this.Controls.Add(this.Advanced);
            this.Controls.Add(this.Local);
            this.Controls.Add(this.Home);
            this.Controls.Add(this.Drive);
            this.Name = "Settings";
            this.Controls.SetChildIndex(this.Drive, 0);
            this.Controls.SetChildIndex(this.Home, 0);
            this.Controls.SetChildIndex(this.Local, 0);
            this.Controls.SetChildIndex(this.Advanced, 0);
            this.Controls.SetChildIndex(this.Next, 0);
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            this.Drive.ResumeLayout(false);
            this.Drive.PerformLayout();
            this.Local.ResumeLayout(false);
            this.Local.PerformLayout();
            this.Home.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Profile;
        private System.Windows.Forms.Label Email;
        private System.Windows.Forms.Button Logout;
        private System.Windows.Forms.Button Drive;
        private System.Windows.Forms.Button Local;
        private System.Windows.Forms.Label LocalLabel;
        private System.Windows.Forms.Label DriveLabel;
        private System.Windows.Forms.Panel Home;
        public System.Windows.Forms.Button Advanced;
        private IconedLabel ClearL;
        private IconedLabel ClearD;
    }
}

