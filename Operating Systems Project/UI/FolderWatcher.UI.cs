using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class FolderWatcher
    {
        private static TextBox _reportTextBox;
        private static Label _statusLabel;
        private static int _eventCount = 0;
        public static void ShowFolderWatcher(Operating_Systems OperatingSystems)
        {
            const int PanelWidth = 1104;
            const int VerticalSpacing = 8;
            int currentY = 0;
            const int LeftPadding = 15;

            Label HeaderLabel = new Label
            {
                Text = "📁 Folder Watcher",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),                
                ForeColor = Operating_Systems.HeaderColor,

                Location = new Point(0, 8),
                Size = new Size(PanelWidth, 30),
            };

            Label SubHeaderLabel = new Label
            {
                Text = "Monitor file system changes in real-time.",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                ForeColor = Operating_Systems.TextPrimary,

                Size = new Size(PanelWidth, 30),
                Location = new Point(0, 38),
            };

            currentY += 47;

            Label labelInputPath = new Label
            {
                Text = "Path",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,

                AutoSize = true,
                Location = new Point(LeftPadding, currentY),
            };

            currentY += labelInputPath.Height + 6;

            Panel pathPanel = new Panel
            {
                Size = new Size(PanelWidth, 36),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 4, 8, 4),

                Location = new Point(LeftPadding, currentY),
            };

            // Making a default Directory
            string defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            TextBox pathTextBox = new TextBox
            {
                Text = defaultDirectory,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = pathPanel.BackColor,
                BorderStyle = BorderStyle.None,

                Size = new Size(PanelWidth - 120, 25),
                Location = new Point(8, 6),
            };

            Button browseButton = new Button
            {
                Text = "Browse",
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,

                Size = new Size(90, 28),
                Location = new Point(PanelWidth - 95, 3),
            };
            browseButton.FlatAppearance.BorderSize = 0;
            browseButton.Click += (s, e) => BrowseForFolder(pathTextBox);

            pathPanel.Controls.Add(pathTextBox);
            pathPanel.Controls.Add(browseButton);
            currentY += pathPanel.Height + VerticalSpacing;

            FlowLayoutPanel optionsFlow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0), // ✅ vertical alignment fix

                Size = new Size(PanelWidth, 30),
                Location = new Point(LeftPadding, currentY),
            };

            CheckBox includeSubdirCheck = new CheckBox
            {
                Text = "Include subdirectories",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                Checked = true,
                AutoSize = true,
                Margin = new Padding(0, 2, 12, 0) // ✅ spacing + alignment
            };

            _statusLabel = new Label
            {
                Text = "⚫ Not Monitoring",
                Font = new Font("Segoe UI Semibold", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Margin = new Padding(0, 4, 12, 0) // ✅ aligns with checkbox baseline
            };

            Label eventCountLabel = new Label
            {
                Name = "eventCountLabel",
                Text = "Events: 0",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Margin = new Padding(0, 4, 0, 0) // ✅ aligns with status label
            };


            optionsFlow.Controls.Add(includeSubdirCheck);
            optionsFlow.Controls.Add(_statusLabel);
            optionsFlow.Controls.Add(eventCountLabel);
            currentY += optionsFlow.Height + VerticalSpacing;

            Panel contentPanel = new Panel
            {
                Location = new Point(LeftPadding, currentY),
                Size = new Size(PanelWidth, 260),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            _reportTextBox = new TextBox
            {
                Font = new Font("Consolas", 10F),
                Multiline = true,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = pathPanel.BackColor,
                ForeColor = Operating_Systems.TextPrimary,
                Text = "Click 'Start Monitoring' to begin tracking file system changes..." // include
            };


            contentPanel.Controls.Add(_reportTextBox);
            currentY += contentPanel.Height + VerticalSpacing;

            int buttonsRowHeight = 46;

            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(LeftPadding, currentY),
                Size = new Size(PanelWidth, buttonsRowHeight),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
            };

            Button startButton = new Button
            {
                Text = "▶ Start",
                Size = new Size(120, 40),
                BackColor = Operating_Systems.AccentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.5F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            startButton.FlatAppearance.BorderSize = 0;

            Button stopButton = new Button
            {
                Text = "⏹ Stop",
                Size = new Size(120, 40),
                BackColor = Operating_Systems.ErrorColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.5F),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(8, 0, 0, 0)
            };
            stopButton.FlatAppearance.BorderSize = 0;

            Button clearButton = new Button
            {
                Text = "Clear Log",
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Margin = new Padding(8, 0, 0, 0)
            };
            clearButton.FlatAppearance.BorderSize = 0;

            buttonFlow.Controls.Add(startButton);
            buttonFlow.Controls.Add(stopButton);
            buttonFlow.Controls.Add(clearButton);

            clearButton.Click += (s, e) =>
            {
                _reportTextBox.Clear();
                _eventCount = 0;
                var label = _reportTextBox.Parent?.Parent?.Controls.Find("eventCountLabel", true);
                if (label?.Length > 0) label[0].Text = "Events: 0";
            };

            startButton.Click += (s, e) =>
            {
                string dir = pathTextBox.Text?.Trim();
                if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                {
                    MessageBox.Show("Please enter a valid existing directory path.",
                        "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pathTextBox.Focus();
                    return;
                }

                StartMonitoring(dir, includeSubdirCheck.Checked);
                _reportTextBox.Clear();
                _statusLabel.Text = "🟢 Monitoring Active";
                _statusLabel.ForeColor = Operating_Systems.SuccessColor;

                AddEvent($"Started Monitoring...\r\n");

                _eventCount = 0;
                startButton.Enabled = false;
                stopButton.Enabled = true;
                pathTextBox.Enabled = false;
                browseButton.Enabled = false;
                includeSubdirCheck.Enabled = false;
            };

            stopButton.Click += (s, e) =>
            {
                StopMonitoring();

                _statusLabel.Text = "⚫ Not Monitoring";
                _statusLabel.ForeColor = Operating_Systems.TextSecondary;

                startButton.Enabled = true;
                stopButton.Enabled = false;
                pathTextBox.Enabled = true;
                browseButton.Enabled = true;
                includeSubdirCheck.Enabled = true;
            };
            OperatingSystems.AddToMainContainer(HeaderLabel);
            OperatingSystems.AddToMainContainer(SubHeaderLabel);
            OperatingSystems.AddToMainContainer(labelInputPath);
            OperatingSystems.AddToMainContainer(pathPanel);
            OperatingSystems.AddToMainContainer(optionsFlow);
            OperatingSystems.AddToMainContainer(contentPanel);
            OperatingSystems.AddToMainContainer(buttonFlow);
        }
    }
}