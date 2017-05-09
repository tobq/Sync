namespace Sync
{
    partial class Drive
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
        /// </summary>private void InitializeComponent()
        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Drive));
            this.Folders = new System.Windows.Forms.FlowLayoutPanel();
            this.Path = new System.Windows.Forms.FlowLayoutPanel();
            this.PathPanel = new System.Windows.Forms.Panel();
            this.Grad = new Sync.Grad();
            this.Info = new Sync.InfoLabel();
            this.PathPanel.SuspendLayout();
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
            this.Next.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            // 
            // Folders
            // 
            this.Folders.AutoScroll = true;
            this.Folders.Location = new System.Drawing.Point(50, 159);
            this.Folders.Margin = new System.Windows.Forms.Padding(0);
            this.Folders.Name = "Folders";
            this.Folders.Size = new System.Drawing.Size(367, 239);
            this.Folders.TabIndex = 11;
            // 
            // Path
            // 
            this.Path.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Path.AutoScroll = true;
            this.Path.Location = new System.Drawing.Point(0, 0);
            this.Path.Margin = new System.Windows.Forms.Padding(0);
            this.Path.Name = "Path";
            this.Path.Size = new System.Drawing.Size(350, 86);
            this.Path.TabIndex = 11;
            this.Path.WrapContents = false;
            // 
            // PathPanel
            // 
            this.PathPanel.Controls.Add(this.Path);
            this.PathPanel.Location = new System.Drawing.Point(50, 107);
            this.PathPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PathPanel.Name = "PathPanel";
            this.PathPanel.Size = new System.Drawing.Size(350, 50);
            this.PathPanel.TabIndex = 0;
            // 
            // Grad
            // 
            this.Grad.Location = new System.Drawing.Point(50, 107);
            this.Grad.Margin = new System.Windows.Forms.Padding(0);
            this.Grad.Name = "Grad";
            this.Grad.Size = new System.Drawing.Size(47, 50);
            this.Grad.TabIndex = 12;
            // 
            // Info
            // 
            this.Info.BackColor = System.Drawing.Color.Transparent;
            this.Info.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Info.ForeColor = System.Drawing.Color.Black;
            this.Info.Icon = ((System.Drawing.Image)(resources.GetObject("Info.Icon")));
            this.Info.IconAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.Info.Location = new System.Drawing.Point(50, 398);
            this.Info.MinimumSize = new System.Drawing.Size(0, 36);
            this.Info.Name = "Info";
            this.Info.Padding = new System.Windows.Forms.Padding(43, 0, 0, 0);
            this.Info.Size = new System.Drawing.Size(350, 64);
            this.Info.TabIndex = 6;
            this.Info.Text = "This Google Drive folder will be automatically synchronised with your local folde" +
    "r";
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Drive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 560);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.Grad);
            this.Controls.Add(this.PathPanel);
            this.Controls.Add(this.Folders);
            this.Name = "Drive";
            this.Text = "Drive";
            this.Controls.SetChildIndex(this.Title, 0);
            this.Controls.SetChildIndex(this.Folders, 0);
            this.Controls.SetChildIndex(this.Next, 0);
            this.Controls.SetChildIndex(this.Back, 0);
            this.Controls.SetChildIndex(this.PathPanel, 0);
            this.Controls.SetChildIndex(this.Grad, 0);
            this.Controls.SetChildIndex(this.Info, 0);
            this.PathPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.FlowLayoutPanel Folders;
        public System.Windows.Forms.FlowLayoutPanel Path;
        private System.Windows.Forms.Panel PathPanel;
        public Grad Grad;
        private InfoLabel Info;
    }
}