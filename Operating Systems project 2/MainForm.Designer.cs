namespace Operating_Systems_Project
{
    partial class Operating_Systems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Operating_Systems));
            this.MainContainer = new System.Windows.Forms.Panel();
            this.SmallHeaderLabel = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.Menu_Closed = new System.Windows.Forms.PictureBox();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.About_Button = new System.Windows.Forms.Button();
            this.PowerOptions_Button = new System.Windows.Forms.Button();
            this.StopwatchTimer_Button = new System.Windows.Forms.Button();
            this.FolderWatcher_Button = new System.Windows.Forms.Button();
            this.WMI_Button = new System.Windows.Forms.Button();
            this.FileReader_Button = new System.Windows.Forms.Button();
            this.FileWriter_Button = new System.Windows.Forms.Button();
            this.OperatingSystemsHeader = new System.Windows.Forms.Label();
            this.Menu_Opened = new System.Windows.Forms.PictureBox();
            this.MainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Closed)).BeginInit();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Opened)).BeginInit();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.Controls.Add(this.SmallHeaderLabel);
            this.MainContainer.Controls.Add(this.HeaderLabel);
            this.MainContainer.Controls.Add(this.Menu_Closed);
            this.MainContainer.Location = new System.Drawing.Point(12, 5);
            this.MainContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Size = new System.Drawing.Size(1110, 533);
            this.MainContainer.TabIndex = 1;
            // 
            // SmallHeaderLabel
            // 
            this.SmallHeaderLabel.AutoSize = true;
            this.SmallHeaderLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SmallHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.SmallHeaderLabel.Location = new System.Drawing.Point(414, 38);
            this.SmallHeaderLabel.MinimumSize = new System.Drawing.Size(300, 20);
            this.SmallHeaderLabel.Name = "SmallHeaderLabel";
            this.SmallHeaderLabel.Size = new System.Drawing.Size(300, 20);
            this.SmallHeaderLabel.TabIndex = 12;
            this.SmallHeaderLabel.Text = "Place Holder, describtion";
            this.SmallHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(130)))));
            this.HeaderLabel.Location = new System.Drawing.Point(457, 8);
            this.HeaderLabel.MinimumSize = new System.Drawing.Size(200, 30);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(200, 30);
            this.HeaderLabel.TabIndex = 11;
            this.HeaderLabel.Text = "📝 Place Holder";
            this.HeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Menu_Closed
            // 
            this.Menu_Closed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.Menu_Closed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Menu_Closed.Image = global::Operating_Systems_project_2.Properties.Resources.Hamburger_menu_white_lines1;
            this.Menu_Closed.Location = new System.Drawing.Point(-5, 0);
            this.Menu_Closed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Menu_Closed.Name = "Menu_Closed";
            this.Menu_Closed.Size = new System.Drawing.Size(26, 25);
            this.Menu_Closed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Menu_Closed.TabIndex = 10;
            this.Menu_Closed.TabStop = false;
            this.Menu_Closed.Click += new System.EventHandler(this.Menu_Closed_Click);
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.BackColor = System.Drawing.Color.Black;
            this.ButtonsPanel.Controls.Add(this.pictureBox1);
            this.ButtonsPanel.Controls.Add(this.About_Button);
            this.ButtonsPanel.Controls.Add(this.PowerOptions_Button);
            this.ButtonsPanel.Controls.Add(this.StopwatchTimer_Button);
            this.ButtonsPanel.Controls.Add(this.FolderWatcher_Button);
            this.ButtonsPanel.Controls.Add(this.WMI_Button);
            this.ButtonsPanel.Controls.Add(this.FileReader_Button);
            this.ButtonsPanel.Controls.Add(this.FileWriter_Button);
            this.ButtonsPanel.Controls.Add(this.OperatingSystemsHeader);
            this.ButtonsPanel.Controls.Add(this.Menu_Opened);
            this.ButtonsPanel.Location = new System.Drawing.Point(-8, 1);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(304, 544);
            this.ButtonsPanel.TabIndex = 2;
            this.ButtonsPanel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Operating_Systems_project_2.Properties.Resources.hacker__1_;
            this.pictureBox1.Location = new System.Drawing.Point(99, 410);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // About_Button
            // 
            this.About_Button.BackColor = System.Drawing.Color.Black;
            this.About_Button.FlatAppearance.BorderSize = 0;
            this.About_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.About_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.About_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.About_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.About_Button.Location = new System.Drawing.Point(20, 358);
            this.About_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.About_Button.Name = "About_Button";
            this.About_Button.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.About_Button.Size = new System.Drawing.Size(272, 45);
            this.About_Button.TabIndex = 22;
            this.About_Button.Text = "     ℹ️ About";
            this.About_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.About_Button.UseVisualStyleBackColor = false;
            this.About_Button.Click += new System.EventHandler(this.About_Button_Click);
            // 
            // PowerOptions_Button
            // 
            this.PowerOptions_Button.BackColor = System.Drawing.Color.Black;
            this.PowerOptions_Button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.PowerOptions_Button.FlatAppearance.BorderSize = 0;
            this.PowerOptions_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.PowerOptions_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PowerOptions_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerOptions_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.PowerOptions_Button.Location = new System.Drawing.Point(20, 305);
            this.PowerOptions_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerOptions_Button.Name = "PowerOptions_Button";
            this.PowerOptions_Button.Size = new System.Drawing.Size(272, 45);
            this.PowerOptions_Button.TabIndex = 21;
            this.PowerOptions_Button.Text = "     🔋 Power Options";
            this.PowerOptions_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PowerOptions_Button.UseVisualStyleBackColor = false;
            this.PowerOptions_Button.Click += new System.EventHandler(this.PowerOptions_Button_Click);
            // 
            // StopwatchTimer_Button
            // 
            this.StopwatchTimer_Button.BackColor = System.Drawing.Color.Black;
            this.StopwatchTimer_Button.FlatAppearance.BorderSize = 0;
            this.StopwatchTimer_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.StopwatchTimer_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopwatchTimer_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopwatchTimer_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.StopwatchTimer_Button.Location = new System.Drawing.Point(20, 199);
            this.StopwatchTimer_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopwatchTimer_Button.Name = "StopwatchTimer_Button";
            this.StopwatchTimer_Button.Size = new System.Drawing.Size(272, 45);
            this.StopwatchTimer_Button.TabIndex = 20;
            this.StopwatchTimer_Button.Text = "     ⏱ Stopwatch Timer";
            this.StopwatchTimer_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StopwatchTimer_Button.UseVisualStyleBackColor = false;
            this.StopwatchTimer_Button.Click += new System.EventHandler(this.StopwatchTimer_Button_Click);
            // 
            // FolderWatcher_Button
            // 
            this.FolderWatcher_Button.BackColor = System.Drawing.Color.Black;
            this.FolderWatcher_Button.FlatAppearance.BorderSize = 0;
            this.FolderWatcher_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.FolderWatcher_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FolderWatcher_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderWatcher_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.FolderWatcher_Button.Location = new System.Drawing.Point(20, 146);
            this.FolderWatcher_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FolderWatcher_Button.Name = "FolderWatcher_Button";
            this.FolderWatcher_Button.Size = new System.Drawing.Size(272, 45);
            this.FolderWatcher_Button.TabIndex = 19;
            this.FolderWatcher_Button.Text = "     📁 Folder Watcher";
            this.FolderWatcher_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FolderWatcher_Button.UseVisualStyleBackColor = false;
            this.FolderWatcher_Button.Click += new System.EventHandler(this.FolderWatcher_Button_Click);
            // 
            // WMI_Button
            // 
            this.WMI_Button.BackColor = System.Drawing.Color.Black;
            this.WMI_Button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.WMI_Button.FlatAppearance.BorderSize = 0;
            this.WMI_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.WMI_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WMI_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WMI_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.WMI_Button.Location = new System.Drawing.Point(20, 252);
            this.WMI_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WMI_Button.Name = "WMI_Button";
            this.WMI_Button.Size = new System.Drawing.Size(272, 45);
            this.WMI_Button.TabIndex = 18;
            this.WMI_Button.Text = "     ⚙️ WMI";
            this.WMI_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WMI_Button.UseVisualStyleBackColor = false;
            this.WMI_Button.Click += new System.EventHandler(this.WMI_Button_Click);
            // 
            // FileReader_Button
            // 
            this.FileReader_Button.BackColor = System.Drawing.Color.Black;
            this.FileReader_Button.FlatAppearance.BorderSize = 0;
            this.FileReader_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.FileReader_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FileReader_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileReader_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.FileReader_Button.Location = new System.Drawing.Point(20, 93);
            this.FileReader_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileReader_Button.Name = "FileReader_Button";
            this.FileReader_Button.Size = new System.Drawing.Size(272, 45);
            this.FileReader_Button.TabIndex = 17;
            this.FileReader_Button.Text = "     📄 File Reader";
            this.FileReader_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileReader_Button.UseVisualStyleBackColor = false;
            this.FileReader_Button.Click += new System.EventHandler(this.FileReader_Button_Click);
            // 
            // FileWriter_Button
            // 
            this.FileWriter_Button.BackColor = System.Drawing.Color.Black;
            this.FileWriter_Button.FlatAppearance.BorderSize = 0;
            this.FileWriter_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.FileWriter_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FileWriter_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileWriter_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.FileWriter_Button.Location = new System.Drawing.Point(20, 40);
            this.FileWriter_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileWriter_Button.Name = "FileWriter_Button";
            this.FileWriter_Button.Size = new System.Drawing.Size(272, 45);
            this.FileWriter_Button.TabIndex = 16;
            this.FileWriter_Button.Text = "     📝 File Writer";
            this.FileWriter_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileWriter_Button.UseVisualStyleBackColor = false;
            this.FileWriter_Button.Click += new System.EventHandler(this.FileWriter_Button_Click);
            // 
            // OperatingSystemsHeader
            // 
            this.OperatingSystemsHeader.AutoSize = true;
            this.OperatingSystemsHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperatingSystemsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.OperatingSystemsHeader.Location = new System.Drawing.Point(58, 509);
            this.OperatingSystemsHeader.Name = "OperatingSystemsHeader";
            this.OperatingSystemsHeader.Size = new System.Drawing.Size(234, 29);
            this.OperatingSystemsHeader.TabIndex = 4;
            this.OperatingSystemsHeader.Text = "Operating Systems";
            // 
            // Menu_Opened
            // 
            this.Menu_Opened.BackColor = System.Drawing.Color.Black;
            this.Menu_Opened.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Menu_Opened.Image = global::Operating_Systems_project_2.Properties.Resources.Hamburger_menu_white_lines1;
            this.Menu_Opened.Location = new System.Drawing.Point(15, 4);
            this.Menu_Opened.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Menu_Opened.Name = "Menu_Opened";
            this.Menu_Opened.Size = new System.Drawing.Size(25, 25);
            this.Menu_Opened.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Menu_Opened.TabIndex = 3;
            this.Menu_Opened.TabStop = false;
            this.Menu_Opened.Click += new System.EventHandler(this.Menu_Opened_Click);
            // 
            // Operating_Systems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(1134, 545);
            this.Controls.Add(this.ButtonsPanel);
            this.Controls.Add(this.MainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Operating_Systems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operating Systems";
            this.MainContainer.ResumeLayout(false);
            this.MainContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Closed)).EndInit();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Opened)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Label OperatingSystemsHeader;
        private System.Windows.Forms.PictureBox Menu_Opened;
        private System.Windows.Forms.Button About_Button;
        private System.Windows.Forms.Button PowerOptions_Button;
        private System.Windows.Forms.Button StopwatchTimer_Button;
        private System.Windows.Forms.Button FolderWatcher_Button;
        private System.Windows.Forms.Button WMI_Button;
        private System.Windows.Forms.Button FileReader_Button;
        private System.Windows.Forms.Button FileWriter_Button;
        private System.Windows.Forms.PictureBox Menu_Closed;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Panel MainContainer;
        private System.Windows.Forms.Label SmallHeaderLabel;
        public System.Windows.Forms.Label HeaderLabel;
    }
}

