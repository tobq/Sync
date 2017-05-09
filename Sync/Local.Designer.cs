namespace Sync
{
    partial class Local
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Local));
            this.Browse = new System.Windows.Forms.Button();
            this.Panel = new System.Windows.Forms.Panel();
            this.Path = new System.Windows.Forms.TextBox();
            this.Folder = new System.Windows.Forms.Label();
            this.Info = new Sync.InfoLabel();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Back
            // 
            this.Back.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Back.FlatAppearance.BorderSize = 0;
            this.Back.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Back.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            // 
            // Next
            // 
            this.Next.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Next.FlatAppearance.BorderSize = 0;
            this.Next.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(230)))));
            this.Next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            // 
            // Browse
            // 
            this.Browse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Browse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Browse.FlatAppearance.BorderSize = 0;
            this.Browse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Browse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Browse.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Browse.ForeColor = System.Drawing.Color.Black;
            this.Browse.Location = new System.Drawing.Point(315, 155);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(85, 40);
            this.Browse.TabIndex = 8;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = false;
            this.Browse.Click += new System.EventHandler(this.BrowseClick);
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.Path);
            this.Panel.Location = new System.Drawing.Point(50, 155);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(265, 40);
            this.Panel.TabIndex = 10;
            this.Panel.Click += new System.EventHandler(this.PanelClick);
            // 
            // Path
            // 
            this.Path.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Path.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Path.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Path.Location = new System.Drawing.Point(19, 11);
            this.Path.Name = "Path";
            this.Path.Size = new System.Drawing.Size(229, 17);
            this.Path.TabIndex = 0;
            this.Path.TextChanged += new System.EventHandler(this.PathTextChanged);
            // 
            // Folder
            // 
            this.Folder.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Folder.Location = new System.Drawing.Point(50, 210);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(350, 40);
            this.Folder.TabIndex = 1;
            this.Folder.Text = "No folder selected";
            this.Folder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Info
            // 
            this.Info.BackColor = System.Drawing.Color.Transparent;
            this.Info.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Info.ForeColor = System.Drawing.Color.Black;
            this.Info.Icon = ((System.Drawing.Image)(resources.GetObject("Info.Icon")));
            this.Info.IconAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.Info.Location = new System.Drawing.Point(50, 382);
            this.Info.MinimumSize = new System.Drawing.Size(0, 36);
            this.Info.Name = "Info";
            this.Info.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.Info.Size = new System.Drawing.Size(350, 80);
            this.Info.TabIndex = 6;
            this.Info.Text = "This folder will be automatically synchronised with your Google Drive";
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Local
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 560);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.Browse);
            this.Name = "Local";
            this.Text = "Local";
            this.Controls.SetChildIndex(this.Title, 0);
            this.Controls.SetChildIndex(this.Back, 0);
            this.Controls.SetChildIndex(this.Next, 0);
            this.Controls.SetChildIndex(this.Browse, 0);
            this.Controls.SetChildIndex(this.Panel, 0);
            this.Controls.SetChildIndex(this.Folder, 0);
            this.Controls.SetChildIndex(this.Info, 0);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.TextBox Path;
        private System.Windows.Forms.Label Folder;
        private InfoLabel Info;
    }
}