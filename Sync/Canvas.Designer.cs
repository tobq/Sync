namespace Sync
{
    partial class Canvas
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
                border.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.Next = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.Min = new System.Windows.Forms.Button();
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driveFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Drag = new System.Windows.Forms.Panel();
            this.TrayMenu.SuspendLayout();
            this.Drag.SuspendLayout();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(133)))), ((int)(((byte)(237)))));
            this.Next.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Next.FlatAppearance.BorderSize = 0;
            this.Next.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Next.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Next.ForeColor = System.Drawing.Color.White;
            this.Next.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Next.Location = new System.Drawing.Point(50, 465);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(350, 45);
            this.Next.TabIndex = 0;
            this.Next.UseVisualStyleBackColor = false;
            this.Next.Click += new System.EventHandler(this.NextClick);
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Exit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Exit.BackgroundImage")));
            this.Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Exit.Dock = System.Windows.Forms.DockStyle.Right;
            this.Exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Exit.FlatAppearance.BorderSize = 0;
            this.Exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Exit.Location = new System.Drawing.Point(-35, 0);
            this.Exit.Margin = new System.Windows.Forms.Padding(0);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(35, 37);
            this.Exit.TabIndex = 1;
            this.Exit.UseVisualStyleBackColor = false;
            this.Exit.Click += new System.EventHandler(this.hide);
            // 
            // Min
            // 
            this.Min.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Min.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Min.BackgroundImage")));
            this.Min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Min.Dock = System.Windows.Forms.DockStyle.Right;
            this.Min.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Min.FlatAppearance.BorderSize = 0;
            this.Min.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Min.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Min.Location = new System.Drawing.Point(-70, 0);
            this.Min.Margin = new System.Windows.Forms.Padding(0);
            this.Min.Name = "Min";
            this.Min.Size = new System.Drawing.Size(35, 37);
            this.Min.TabIndex = 1;
            this.Min.UseVisualStyleBackColor = false;
            this.Min.Click += new System.EventHandler(this.MinClick);
            // 
            // TrayMenu
            // 
            this.TrayMenu.BackColor = System.Drawing.Color.White;
            this.TrayMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TrayMenu.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.TrayMenu.Name = "TrayMenu";
            this.TrayMenu.Size = new System.Drawing.Size(108, 48);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localFolderToolStripMenuItem,
            this.driveFolderToolStripMenuItem});
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openFolderToolStripMenuItem.Text = "Open";
            // 
            // localFolderToolStripMenuItem
            // 
            this.localFolderToolStripMenuItem.Name = "localFolderToolStripMenuItem";
            this.localFolderToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.localFolderToolStripMenuItem.Text = "Local Folder";
            // 
            // driveFolderToolStripMenuItem
            // 
            this.driveFolderToolStripMenuItem.Name = "driveFolderToolStripMenuItem";
            this.driveFolderToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.driveFolderToolStripMenuItem.Text = "Drive Folder";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += Program.Close;
            // 
            // Drag
            // 
            this.Drag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Drag.Controls.Add(this.Min);
            this.Drag.Controls.Add(this.Exit);
            this.Drag.Dock = System.Windows.Forms.DockStyle.Top;
            this.Drag.Location = new System.Drawing.Point(2, 2);
            this.Drag.Margin = new System.Windows.Forms.Padding(0);
            this.Drag.Name = "Drag";
            this.Drag.Size = new System.Drawing.Size(0, 37);
            this.Drag.TabIndex = 5;
            this.Drag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragDown);
            this.Drag.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragMove);
            this.Drag.MouseUp += new System.Windows.Forms.MouseEventHandler(this.onMouseUp);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1, 0);
            this.ControlBox = false;
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Drag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Canvas";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "Form1";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.onMouseUp);
            this.TrayMenu.ResumeLayout(false);
            this.Drag.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button Min;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem driveFolderToolStripMenuItem;
        private System.Windows.Forms.Panel Drag;
    }
}

