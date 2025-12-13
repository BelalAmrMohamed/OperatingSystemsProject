namespace Operating_Systems_Project // red background
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
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.General_IO_Button = new System.Windows.Forms.Button();
            this.Menu_Button2 = new System.Windows.Forms.PictureBox();
            this.Hacker_Box = new System.Windows.Forms.PictureBox();
            this.Settings_Button = new System.Windows.Forms.Button();
            this.PowerOptions_Button = new System.Windows.Forms.Button();
            this.StopwatchTimer_Button = new System.Windows.Forms.Button();
            this.FolderWatcher_Button = new System.Windows.Forms.Button();
            this.WMI_Button = new System.Windows.Forms.Button();
            this.FileReader_Button = new System.Windows.Forms.Button();
            this.FileWriter_Button = new System.Windows.Forms.Button();
            this.Menu_Button = new System.Windows.Forms.PictureBox();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Button2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Hacker_Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Button)).BeginInit();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.BackColor = System.Drawing.Color.Transparent;
            this.MainContainer.Location = new System.Drawing.Point(12, 5);
            this.MainContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Size = new System.Drawing.Size(1110, 533);
            this.MainContainer.TabIndex = 1;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ButtonsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ButtonsPanel.Controls.Add(this.General_IO_Button);
            this.ButtonsPanel.Controls.Add(this.Menu_Button2);
            this.ButtonsPanel.Controls.Add(this.Hacker_Box);
            this.ButtonsPanel.Controls.Add(this.Settings_Button);
            this.ButtonsPanel.Controls.Add(this.PowerOptions_Button);
            this.ButtonsPanel.Controls.Add(this.StopwatchTimer_Button);
            this.ButtonsPanel.Controls.Add(this.FolderWatcher_Button);
            this.ButtonsPanel.Controls.Add(this.WMI_Button);
            this.ButtonsPanel.Controls.Add(this.FileReader_Button);
            this.ButtonsPanel.Controls.Add(this.FileWriter_Button);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(280, 545);
            this.ButtonsPanel.TabIndex = 2;
            this.ButtonsPanel.Visible = false;
            // 
            // General_IO_Button
            // 
            this.General_IO_Button.BackColor = System.Drawing.Color.Transparent;
            this.General_IO_Button.FlatAppearance.BorderSize = 0;
            this.General_IO_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.General_IO_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.General_IO_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.General_IO_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.General_IO_Button.ForeColor = System.Drawing.Color.White;
            this.General_IO_Button.Location = new System.Drawing.Point(0, 30);
            this.General_IO_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.General_IO_Button.Name = "General_IO_Button";
            this.General_IO_Button.Size = new System.Drawing.Size(280, 45);
            this.General_IO_Button.TabIndex = 27;
            this.General_IO_Button.Text = "     🔁 General IO";
            this.General_IO_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.General_IO_Button.UseVisualStyleBackColor = false;
            this.General_IO_Button.Click += new System.EventHandler(this.General_IO_Button_Click);
            // 
            // Menu_Button2
            // 
            this.Menu_Button2.BackColor = System.Drawing.Color.Transparent;
            this.Menu_Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Menu_Button2.Image = global::Operating_Systems_Project.Properties.Resources.Hamburger_menu_white_lines;
            this.Menu_Button2.Location = new System.Drawing.Point(2, 2);
            this.Menu_Button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Menu_Button2.Name = "Menu_Button2";
            this.Menu_Button2.Size = new System.Drawing.Size(30, 30);
            this.Menu_Button2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Menu_Button2.TabIndex = 26;
            this.Menu_Button2.TabStop = false;
            this.Menu_Button2.Click += new System.EventHandler(this.Menu_Button2_Click);
            // 
            // Hacker_Box
            // 
            this.Hacker_Box.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Hacker_Box.Image = global::Operating_Systems_Project.Properties.Resources.AppImage;
            this.Hacker_Box.Location = new System.Drawing.Point(0, 425);
            this.Hacker_Box.Name = "Hacker_Box";
            this.Hacker_Box.Size = new System.Drawing.Size(278, 118);
            this.Hacker_Box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Hacker_Box.TabIndex = 23;
            this.Hacker_Box.TabStop = false;
            this.Hacker_Box.Click += new System.EventHandler(this.Hacker_Box_Click);
            // 
            // Settings_Button
            // 
            this.Settings_Button.BackColor = System.Drawing.Color.Transparent;
            this.Settings_Button.FlatAppearance.BorderSize = 0;
            this.Settings_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Settings_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.Settings_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Settings_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_Button.ForeColor = System.Drawing.Color.White;
            this.Settings_Button.Location = new System.Drawing.Point(0, 380);
            this.Settings_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Settings_Button.Name = "Settings_Button";
            this.Settings_Button.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Settings_Button.Size = new System.Drawing.Size(280, 45);
            this.Settings_Button.TabIndex = 22;
            this.Settings_Button.Text = "     ⚙️ Settings";
            this.Settings_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Settings_Button.UseVisualStyleBackColor = false;
            this.Settings_Button.Click += new System.EventHandler(this.Settings_Button_Click);
            // 
            // PowerOptions_Button
            // 
            this.PowerOptions_Button.BackColor = System.Drawing.Color.Transparent;
            this.PowerOptions_Button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.PowerOptions_Button.FlatAppearance.BorderSize = 0;
            this.PowerOptions_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.PowerOptions_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.PowerOptions_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PowerOptions_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerOptions_Button.ForeColor = System.Drawing.Color.White;
            this.PowerOptions_Button.Location = new System.Drawing.Point(0, 330);
            this.PowerOptions_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PowerOptions_Button.Name = "PowerOptions_Button";
            this.PowerOptions_Button.Size = new System.Drawing.Size(280, 45);
            this.PowerOptions_Button.TabIndex = 21;
            this.PowerOptions_Button.Text = "     🔋 Power Options";
            this.PowerOptions_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PowerOptions_Button.UseVisualStyleBackColor = false;
            this.PowerOptions_Button.Click += new System.EventHandler(this.PowerOptions_Button_Click);
            // 
            // StopwatchTimer_Button
            // 
            this.StopwatchTimer_Button.BackColor = System.Drawing.Color.Transparent;
            this.StopwatchTimer_Button.FlatAppearance.BorderSize = 0;
            this.StopwatchTimer_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.StopwatchTimer_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.StopwatchTimer_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopwatchTimer_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopwatchTimer_Button.ForeColor = System.Drawing.Color.White;
            this.StopwatchTimer_Button.Location = new System.Drawing.Point(0, 230);
            this.StopwatchTimer_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopwatchTimer_Button.Name = "StopwatchTimer_Button";
            this.StopwatchTimer_Button.Size = new System.Drawing.Size(280, 45);
            this.StopwatchTimer_Button.TabIndex = 20;
            this.StopwatchTimer_Button.Text = "     ⏱ Stopwatch Timer";
            this.StopwatchTimer_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StopwatchTimer_Button.UseVisualStyleBackColor = false;
            this.StopwatchTimer_Button.Click += new System.EventHandler(this.StopwatchTimer_Button_Click);
            // 
            // FolderWatcher_Button
            // 
            this.FolderWatcher_Button.BackColor = System.Drawing.Color.Transparent;
            this.FolderWatcher_Button.FlatAppearance.BorderSize = 0;
            this.FolderWatcher_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.FolderWatcher_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.FolderWatcher_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FolderWatcher_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FolderWatcher_Button.ForeColor = System.Drawing.Color.White;
            this.FolderWatcher_Button.Location = new System.Drawing.Point(0, 180);
            this.FolderWatcher_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FolderWatcher_Button.Name = "FolderWatcher_Button";
            this.FolderWatcher_Button.Size = new System.Drawing.Size(280, 45);
            this.FolderWatcher_Button.TabIndex = 19;
            this.FolderWatcher_Button.Text = "     📁 Folder Watcher";
            this.FolderWatcher_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FolderWatcher_Button.UseVisualStyleBackColor = false;
            this.FolderWatcher_Button.Click += new System.EventHandler(this.FolderWatcher_Button_Click);
            // 
            // WMI_Button
            // 
            this.WMI_Button.BackColor = System.Drawing.Color.Transparent;
            this.WMI_Button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.WMI_Button.FlatAppearance.BorderSize = 0;
            this.WMI_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.WMI_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.WMI_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WMI_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WMI_Button.ForeColor = System.Drawing.Color.White;
            this.WMI_Button.Location = new System.Drawing.Point(0, 280);
            this.WMI_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WMI_Button.Name = "WMI_Button";
            this.WMI_Button.Size = new System.Drawing.Size(280, 45);
            this.WMI_Button.TabIndex = 18;
            this.WMI_Button.Text = "     ⚙️ WMI";
            this.WMI_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WMI_Button.UseVisualStyleBackColor = false;
            this.WMI_Button.Click += new System.EventHandler(this.WMI_Button_Click);
            // 
            // FileReader_Button
            // 
            this.FileReader_Button.BackColor = System.Drawing.Color.Transparent;
            this.FileReader_Button.FlatAppearance.BorderSize = 0;
            this.FileReader_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.FileReader_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.FileReader_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FileReader_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileReader_Button.ForeColor = System.Drawing.Color.White;
            this.FileReader_Button.Location = new System.Drawing.Point(0, 130);
            this.FileReader_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileReader_Button.Name = "FileReader_Button";
            this.FileReader_Button.Size = new System.Drawing.Size(280, 45);
            this.FileReader_Button.TabIndex = 17;
            this.FileReader_Button.Text = "     📄 File Reader";
            this.FileReader_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileReader_Button.UseVisualStyleBackColor = false;
            this.FileReader_Button.Click += new System.EventHandler(this.FileReader_Button_Click);
            // 
            // FileWriter_Button
            // 
            this.FileWriter_Button.BackColor = System.Drawing.Color.Transparent;
            this.FileWriter_Button.FlatAppearance.BorderSize = 0;
            this.FileWriter_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.FileWriter_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.FileWriter_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FileWriter_Button.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileWriter_Button.ForeColor = System.Drawing.Color.White;
            this.FileWriter_Button.Location = new System.Drawing.Point(0, 80);
            this.FileWriter_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileWriter_Button.Name = "FileWriter_Button";
            this.FileWriter_Button.Size = new System.Drawing.Size(280, 45);
            this.FileWriter_Button.TabIndex = 16;
            this.FileWriter_Button.Text = "     📝 File Writer";
            this.FileWriter_Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileWriter_Button.UseVisualStyleBackColor = false;
            this.FileWriter_Button.Click += new System.EventHandler(this.FileWriter_Button_Click);
            // 
            // Menu_Button
            // 
            this.Menu_Button.BackColor = System.Drawing.Color.Transparent;
            this.Menu_Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Menu_Button.Image = global::Operating_Systems_Project.Properties.Resources.Hamburger_menu_white_lines;
            this.Menu_Button.Location = new System.Drawing.Point(2, 2);
            this.Menu_Button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Menu_Button.Name = "Menu_Button";
            this.Menu_Button.Size = new System.Drawing.Size(30, 30);
            this.Menu_Button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Menu_Button.TabIndex = 25;
            this.Menu_Button.TabStop = false;
            this.Menu_Button.Click += new System.EventHandler(this.Menu_Button_Click);
            // 
            // Operating_Systems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1134, 545);
            this.Controls.Add(this.ButtonsPanel);
            this.Controls.Add(this.Menu_Button);
            this.Controls.Add(this.MainContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Operating_Systems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operating Systems";
            this.Load += new System.EventHandler(this.Operating_Systems_Load);
            this.ButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Button2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Hacker_Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Menu_Button)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Button Settings_Button;
        private System.Windows.Forms.Button PowerOptions_Button;
        private System.Windows.Forms.Button StopwatchTimer_Button;
        private System.Windows.Forms.Button FolderWatcher_Button;
        private System.Windows.Forms.Button WMI_Button;
        private System.Windows.Forms.Button FileReader_Button;
        private System.Windows.Forms.Button FileWriter_Button;
        private System.Windows.Forms.PictureBox Hacker_Box;
        public System.Windows.Forms.Panel MainContainer;
        private System.Windows.Forms.PictureBox Menu_Button;
        private System.Windows.Forms.PictureBox Menu_Button2;
        private System.Windows.Forms.Button General_IO_Button;
    }
}

