namespace Sync
{
    partial class Advanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Advanced));
            this.From = new System.Windows.Forms.Label();
            this.To = new System.Windows.Forms.Label();
            this.ToLabel = new System.Windows.Forms.Label();
            this.FromH = new Sync.TimeList();
            this.FromM = new Sync.TimeList();
            this.ToH = new Sync.TimeList();
            this.ToM = new Sync.TimeList();
            this.FromPanel = new System.Windows.Forms.Panel();
            this.FromMLabel = new System.Windows.Forms.Label();
            this.FromHLabel = new System.Windows.Forms.Label();
            this.ToPanel = new System.Windows.Forms.Panel();
            this.ToMLabel = new System.Windows.Forms.Label();
            this.ToHLabel = new System.Windows.Forms.Label();
            this.QuietHours = new Sync.SettingTitle();
            this.Clear = new Sync.IconedLabel();
            this.Priority = new Sync.SettingTitle();
            this.AddPriority = new Sync.IconedLabel();
            this.prioritised = new Sync.Advanced.PriorityPanel();
            this.FromPanel.SuspendLayout();
            this.ToPanel.SuspendLayout();
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
            // From
            // 
            this.From.BackColor = System.Drawing.Color.Transparent;
            this.From.Font = new System.Drawing.Font("Arial", 16F);
            this.From.ForeColor = System.Drawing.Color.Black;
            this.From.Image = ((System.Drawing.Image)(resources.GetObject("From.Image")));
            this.From.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.From.Location = new System.Drawing.Point(50, 175);
            this.From.Name = "From";
            this.From.Size = new System.Drawing.Size(160, 44);
            this.From.TabIndex = 6;
            this.From.Text = "00:00";
            this.From.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.From.Click += new System.EventHandler(this.ToggleFrom);
            // 
            // To
            // 
            this.To.BackColor = System.Drawing.Color.Transparent;
            this.To.Font = new System.Drawing.Font("Arial", 16F);
            this.To.ForeColor = System.Drawing.Color.Black;
            this.To.Image = ((System.Drawing.Image)(resources.GetObject("To.Image")));
            this.To.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.To.Location = new System.Drawing.Point(240, 175);
            this.To.Name = "To";
            this.To.Size = new System.Drawing.Size(160, 44);
            this.To.TabIndex = 6;
            this.To.Text = "00:00";
            this.To.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.To.Click += new System.EventHandler(this.ToggleTo);
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ToLabel.Location = new System.Drawing.Point(213, 183);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(27, 23);
            this.ToLabel.TabIndex = 6;
            this.ToLabel.Text = "to";
            // 
            // FromH
            // 
            this.FromH.BackColor = System.Drawing.Color.White;
            this.FromH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FromH.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FromH.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FromH.ForeColor = System.Drawing.Color.Black;
            this.FromH.FormattingEnabled = true;
            this.FromH.ItemHeight = 30;
            this.FromH.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.FromH.Location = new System.Drawing.Point(0, 25);
            this.FromH.Margin = new System.Windows.Forms.Padding(0);
            this.FromH.Name = "FromH";
            this.FromH.Size = new System.Drawing.Size(73, 120);
            this.FromH.TabIndex = 9;
            this.FromH.Click += new System.EventHandler(this.ListClick);
            // 
            // FromM
            // 
            this.FromM.BackColor = System.Drawing.Color.White;
            this.FromM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FromM.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FromM.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FromM.ForeColor = System.Drawing.Color.Black;
            this.FromM.FormattingEnabled = true;
            this.FromM.ItemHeight = 30;
            this.FromM.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.FromM.Location = new System.Drawing.Point(73, 25);
            this.FromM.Margin = new System.Windows.Forms.Padding(0);
            this.FromM.Name = "FromM";
            this.FromM.Size = new System.Drawing.Size(73, 120);
            this.FromM.TabIndex = 9;
            this.FromM.Click += new System.EventHandler(this.ListClick);
            // 
            // ToH
            // 
            this.ToH.BackColor = System.Drawing.Color.White;
            this.ToH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToH.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ToH.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ToH.ForeColor = System.Drawing.Color.Black;
            this.ToH.FormattingEnabled = true;
            this.ToH.ItemHeight = 30;
            this.ToH.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.ToH.Location = new System.Drawing.Point(0, 25);
            this.ToH.Margin = new System.Windows.Forms.Padding(0);
            this.ToH.Name = "ToH";
            this.ToH.Size = new System.Drawing.Size(73, 120);
            this.ToH.TabIndex = 9;
            this.ToH.Click += new System.EventHandler(this.ListClick);
            // 
            // ToM
            // 
            this.ToM.BackColor = System.Drawing.Color.White;
            this.ToM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToM.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ToM.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ToM.ForeColor = System.Drawing.Color.Black;
            this.ToM.FormattingEnabled = true;
            this.ToM.ItemHeight = 30;
            this.ToM.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.ToM.Location = new System.Drawing.Point(73, 25);
            this.ToM.Margin = new System.Windows.Forms.Padding(0);
            this.ToM.Name = "ToM";
            this.ToM.Size = new System.Drawing.Size(73, 120);
            this.ToM.TabIndex = 9;
            this.ToM.Click += new System.EventHandler(this.ListClick);
            // 
            // FromPanel
            // 
            this.FromPanel.AutoSize = true;
            this.FromPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FromPanel.Controls.Add(this.FromMLabel);
            this.FromPanel.Controls.Add(this.FromHLabel);
            this.FromPanel.Controls.Add(this.FromH);
            this.FromPanel.Controls.Add(this.FromM);
            this.FromPanel.Location = new System.Drawing.Point(50, 226);
            this.FromPanel.Name = "FromPanel";
            this.FromPanel.Size = new System.Drawing.Size(149, 145);
            this.FromPanel.TabIndex = 10;
            this.FromPanel.Visible = false;
            // 
            // FromMLabel
            // 
            this.FromMLabel.BackColor = System.Drawing.Color.White;
            this.FromMLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.FromMLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.FromMLabel.Location = new System.Drawing.Point(73, 0);
            this.FromMLabel.Name = "FromMLabel";
            this.FromMLabel.Size = new System.Drawing.Size(73, 25);
            this.FromMLabel.TabIndex = 10;
            this.FromMLabel.Text = "MM";
            this.FromMLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FromHLabel
            // 
            this.FromHLabel.BackColor = System.Drawing.Color.White;
            this.FromHLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.FromHLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.FromHLabel.Location = new System.Drawing.Point(0, 0);
            this.FromHLabel.Name = "FromHLabel";
            this.FromHLabel.Size = new System.Drawing.Size(73, 25);
            this.FromHLabel.TabIndex = 10;
            this.FromHLabel.Text = "HH";
            this.FromHLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToPanel
            // 
            this.ToPanel.AutoSize = true;
            this.ToPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ToPanel.Controls.Add(this.ToMLabel);
            this.ToPanel.Controls.Add(this.ToH);
            this.ToPanel.Controls.Add(this.ToHLabel);
            this.ToPanel.Controls.Add(this.ToM);
            this.ToPanel.Location = new System.Drawing.Point(251, 226);
            this.ToPanel.Name = "ToPanel";
            this.ToPanel.Size = new System.Drawing.Size(149, 145);
            this.ToPanel.TabIndex = 10;
            this.ToPanel.Visible = false;
            // 
            // ToMLabel
            // 
            this.ToMLabel.BackColor = System.Drawing.Color.White;
            this.ToMLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.ToMLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ToMLabel.Location = new System.Drawing.Point(73, 0);
            this.ToMLabel.Name = "ToMLabel";
            this.ToMLabel.Size = new System.Drawing.Size(73, 25);
            this.ToMLabel.TabIndex = 10;
            this.ToMLabel.Text = "MM";
            this.ToMLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToHLabel
            // 
            this.ToHLabel.BackColor = System.Drawing.Color.White;
            this.ToHLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.ToHLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ToHLabel.Location = new System.Drawing.Point(0, 0);
            this.ToHLabel.Name = "ToHLabel";
            this.ToHLabel.Size = new System.Drawing.Size(73, 25);
            this.ToHLabel.TabIndex = 10;
            this.ToHLabel.Text = "HH";
            this.ToHLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QuietHours
            // 
            this.QuietHours.AutoSize = true;
            this.QuietHours.BackColor = System.Drawing.Color.Transparent;
            this.QuietHours.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.QuietHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.QuietHours.Icon = global::Sync.Properties.Resources.Info;
            this.QuietHours.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.QuietHours.Location = new System.Drawing.Point(50, 133);
            this.QuietHours.MinimumSize = new System.Drawing.Size(0, 24);
            this.QuietHours.Name = "QuietHours";
            this.QuietHours.Padding = new System.Windows.Forms.Padding(0, 0, 29, 0);
            this.QuietHours.Size = new System.Drawing.Size(119, 24);
            this.QuietHours.TabIndex = 6;
            this.QuietHours.Text = "Quiet Hours";
            this.QuietHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.QuietHours.Tip = "Within quiet hours, the application will maintain a\r\n snoozed state, in which no " +
    "synchronisation occurs";
            // 
            // Clear
            // 
            this.Clear.AutoSize = true;
            this.Clear.BackColor = System.Drawing.Color.Transparent;
            this.Clear.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Clear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.Clear.Icon = null;
            this.Clear.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Clear.Location = new System.Drawing.Point(354, 133);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(46, 18);
            this.Clear.TabIndex = 6;
            this.Clear.Text = "Clear";
            this.Clear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Clear.Click += new System.EventHandler(this.clearTimes);
            // 
            // Priority
            // 
            this.Priority.AutoSize = true;
            this.Priority.BackColor = System.Drawing.Color.Transparent;
            this.Priority.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Priority.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.Priority.Icon = global::Sync.Properties.Resources.Info;
            this.Priority.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Priority.Location = new System.Drawing.Point(50, 248);
            this.Priority.MinimumSize = new System.Drawing.Size(0, 24);
            this.Priority.Name = "Priority";
            this.Priority.Padding = new System.Windows.Forms.Padding(0, 0, 29, 0);
            this.Priority.Size = new System.Drawing.Size(86, 24);
            this.Priority.TabIndex = 6;
            this.Priority.Text = "Priority";
            this.Priority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Priority.Tip = "Select and order items you wish to\r\nhave priority in synchronisation";
            // 
            // AddPriority
            // 
            this.AddPriority.AutoSize = true;
            this.AddPriority.BackColor = System.Drawing.Color.Transparent;
            this.AddPriority.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AddPriority.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.AddPriority.Icon = global::Sync.Properties.Resources.Add;
            this.AddPriority.IconAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AddPriority.Location = new System.Drawing.Point(339, 248);
            this.AddPriority.MinimumSize = new System.Drawing.Size(0, 24);
            this.AddPriority.Name = "AddPriority";
            this.AddPriority.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.AddPriority.Size = new System.Drawing.Size(61, 24);
            this.AddPriority.TabIndex = 6;
            this.AddPriority.Text = "Add";
            this.AddPriority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AddPriority.Click += new System.EventHandler(this.newPrioritised);
            // 
            // prioritised
            // 
            this.prioritised.AllowDrop = true;
            this.prioritised.AutoScroll = true;
            this.prioritised.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.prioritised.Location = new System.Drawing.Point(50, 293);
            this.prioritised.Name = "prioritised";
            this.prioritised.Size = new System.Drawing.Size(350, 163);
            this.prioritised.TabIndex = 12;
            // 
            // Advanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 560);
            this.Controls.Add(this.ToPanel);
            this.Controls.Add(this.FromPanel);
            this.Controls.Add(this.prioritised);
            this.Controls.Add(this.AddPriority);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Priority);
            this.Controls.Add(this.QuietHours);
            this.Controls.Add(this.ToLabel);
            this.Controls.Add(this.To);
            this.Controls.Add(this.From);
            this.Name = "Advanced";
            this.Text = "Drive";
            this.Controls.SetChildIndex(this.From, 0);
            this.Controls.SetChildIndex(this.To, 0);
            this.Controls.SetChildIndex(this.ToLabel, 0);
            this.Controls.SetChildIndex(this.QuietHours, 0);
            this.Controls.SetChildIndex(this.Priority, 0);
            this.Controls.SetChildIndex(this.Clear, 0);
            this.Controls.SetChildIndex(this.AddPriority, 0);
            this.Controls.SetChildIndex(this.prioritised, 0);
            this.Controls.SetChildIndex(this.Title, 0);
            this.Controls.SetChildIndex(this.Next, 0);
            this.Controls.SetChildIndex(this.Back, 0);
            this.Controls.SetChildIndex(this.FromPanel, 0);
            this.Controls.SetChildIndex(this.ToPanel, 0);
            this.FromPanel.ResumeLayout(false);
            this.ToPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label From;
        private System.Windows.Forms.Label To;
        private System.Windows.Forms.Label ToLabel;
        private TimeList FromH;
        private TimeList FromM;
        private TimeList ToH;
        private TimeList ToM;
        private System.Windows.Forms.Panel FromPanel;
        private System.Windows.Forms.Panel ToPanel;
        private System.Windows.Forms.Label FromMLabel;
        private System.Windows.Forms.Label FromHLabel;
        private System.Windows.Forms.Label ToMLabel;
        private System.Windows.Forms.Label ToHLabel;
        private SettingTitle QuietHours;
        private IconedLabel Clear;
        private SettingTitle Priority;
        private IconedLabel AddPriority;
        private PriorityPanel prioritised;
    }
}