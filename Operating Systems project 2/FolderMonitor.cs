using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class FolderMonitor
    {
        private static FileSystemWatcher _watcher;
        private static TextBox _reportTextBox;
        private static Label _statusLabel;
        private static int _eventCount = 0;

        #region Watcher Methods
        private static void StartMonitoring(string path, bool includeSubdirectories)
        {
            StopMonitoring();

            _watcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName |
                               NotifyFilters.LastWrite | NotifyFilters.Size,
                IncludeSubdirectories = includeSubdirectories,
                EnableRaisingEvents = true
            };

            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;
        }

        private static void StopMonitoring()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= OnChanged;
                _watcher.Created -= OnCreated;
                _watcher.Deleted -= OnDeleted;
                _watcher.Renamed -= OnRenamed;
                _watcher.Error -= OnError;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private static void LogEvent(string message, string icon = "📝")
        {
            if (_reportTextBox == null || _reportTextBox.IsDisposed) return;

            if (_reportTextBox.InvokeRequired)
            {
                _reportTextBox.Invoke(new Action(() => LogEvent(message, icon)));
                return;
            }

            _eventCount++;
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            _reportTextBox.AppendText($"[{timestamp}] {icon} {message}\r\n");
            _reportTextBox.SelectionStart = _reportTextBox.Text.Length;
            _reportTextBox.ScrollToCaret();

            // Update event count label (search the holder)
            Control parentHolder = _reportTextBox.Parent?.Parent;
            if (parentHolder != null)
            {
                var found = parentHolder.Controls.Find("eventCountLabel", true);
                if (found.Length > 0 && found[0] is Label eventLabel)
                    eventLabel.Text = $"Events: {_eventCount}";
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e) => LogEvent($"Modified: {e.Name}", "🔄");
        private static void OnDeleted(object sender, FileSystemEventArgs e) => LogEvent($"Deleted: {e.Name}", "🗑️");
        private static void OnRenamed(object sender, RenamedEventArgs e) => LogEvent($"Renamed: {e.OldName} → {e.Name}", "🔄");
        private static void OnError(object sender, ErrorEventArgs e) => LogEvent($"Error: {e.GetException()?.Message}", "❌");
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string type = Directory.Exists(e.FullPath) ? "Folder" : "File";
            LogEvent($"Created: {e.Name} ({type})", "✅");
        }

        private static void ExportLog()
        {
            if (_reportTextBox == null || string.IsNullOrWhiteSpace(_reportTextBox.Text))
            {
                MessageBox.Show("No log data to export.", "Export Log",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.Title = "Export Activity Log";
                saveDialog.FileName = $"FolderMonitor_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveDialog.FileName, _reportTextBox.Text);
                        MessageBox.Show($"Log exported successfully to:\n{saveDialog.FileName}",
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export log:\n{ex.Message}",
                            "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        public static void ShowFolderMonitor(Operating_Systems OperatingSystems)
        {
            StopMonitoring();

            // Layout constants (optimized and compressed)
            const int PanelWidth = 1104;
            const int VerticalSpacing = 8;
            int currentY = 0;

            // Center flow & fixed-width content holder (height = container's height ~ 482)
            FlowLayoutPanel centerFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Operating_Systems.Background,
                AutoScroll = false
            };

            Panel contentHolder = new Panel
            {
                Size = new Size(PanelWidth, 482),
                BackColor = Operating_Systems.Background,
                Margin = new Padding(0)
            };            
            currentY += 47;            

            // Path label
            Label labelInputPath = new Label
            {
                Text = "Path",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += labelInputPath.Height + 6;

            // Path panel (full width, compact height)
            Panel pathPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 36),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 4, 8, 4)
            };

            TextBox pathTextBox = new TextBox
            {
                Name = "pathTextBox",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.None,
                Location = new Point(8, 6),
                Size = new Size(PanelWidth - 120, 25),
                Text = $@"C:\Users\{Environment.UserName}\Downloads"
            };

            Button browseButton = new Button
            {
                Text = "Browse",
                Size = new Size(90, 28),
                Location = new Point(PanelWidth - 95, 3),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            browseButton.FlatAppearance.BorderSize = 0;
            browseButton.Click += (s, e) => BrowseForFolder(pathTextBox);

            pathPanel.Controls.Add(pathTextBox);
            pathPanel.Controls.Add(browseButton);
            currentY += pathPanel.Height + VerticalSpacing;

            // Options + status row (compact, fits horizontally)
            FlowLayoutPanel optionsFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 30),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
            };

            CheckBox includeSubdirCheck = new CheckBox
            {
                Text = "Include subdirectories",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Checked = true,
                Margin = new Padding(0, 6, 12, 0)
            };

            _statusLabel = new Label
            {
                Text = "⚫ Not Monitoring",
                Font = new Font("Segoe UI Semibold", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Margin = new Padding(6, 6, 12, 0)
            };

            Label eventCountLabel = new Label
            {
                Name = "eventCountLabel",
                Text = "Events: 0",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Margin = new Padding(6, 6, 0, 0)
            };

            // Add them to options flow
            optionsFlow.Controls.Add(includeSubdirCheck);
            optionsFlow.Controls.Add(_statusLabel);
            optionsFlow.Controls.Add(eventCountLabel);

            currentY += optionsFlow.Height + VerticalSpacing;

            // Content label
            Label contentLabel = new Label
            {
                Text = "Activity Log",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += contentLabel.Height + 6;

            // Content panel occupies remaining height minus buttons row (we compute dynamically)
            int buttonsRowHeight = 46;
            int contentPanelHeight = contentHolder.Height - currentY - buttonsRowHeight - (VerticalSpacing * 2);
            if (contentPanelHeight < 120) contentPanelHeight = 120; // minimal reasonable height

            Panel contentPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, contentPanelHeight - 58),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            _reportTextBox = new TextBox
            {
                Name = "reportTextBox",
                Font = new Font("Consolas", 10F),
                Multiline = true,
                ScrollBars = ScrollBars.None,
                WordWrap = false,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                Text = "Click 'Start Monitoring' to begin tracking file system changes..."
            };

            contentPanel.Controls.Add(_reportTextBox);
            currentY += contentPanel.Height + VerticalSpacing + 25;

            // Buttons row (compact)
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, buttonsRowHeight),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
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

            Button exportButton = new Button
            {
                Text = "💾 Export",
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Margin = new Padding(8, 0, 0, 0)
            };
            exportButton.FlatAppearance.BorderSize = 0;

            buttonFlow.Controls.Add(startButton);
            buttonFlow.Controls.Add(stopButton);
            buttonFlow.Controls.Add(clearButton);
            buttonFlow.Controls.Add(exportButton);

            // Wiring events (compact behavior preserved)
            clearButton.Click += (s, e) =>
            {
                _reportTextBox.Clear();
                _eventCount = 0;
                eventCountLabel.Text = "Events: 0";
            };

            exportButton.Click += (s, e) => ExportLog();

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
                _reportTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Started monitoring: {dir}\r\n");
                _reportTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Include subdirectories: {includeSubdirCheck.Checked}\r\n");
                _reportTextBox.AppendText(new string('-', 80) + "\r\n");

                _statusLabel.Text = "🟢 Monitoring Active";
                _statusLabel.ForeColor = Operating_Systems.SuccessColor;
                _eventCount = 0;
                eventCountLabel.Text = "Events: 0";

                startButton.Enabled = false;
                stopButton.Enabled = true;
                pathTextBox.Enabled = false;
                browseButton.Enabled = false;
                includeSubdirCheck.Enabled = false;
            };

            stopButton.Click += (s, e) =>
            {
                StopMonitoring();
                _reportTextBox.AppendText($"\r\n[{DateTime.Now:HH:mm:ss}] Monitoring stopped.\r\n");

                _statusLabel.Text = "⚫ Not Monitoring";
                _statusLabel.ForeColor = Operating_Systems.TextSecondary;

                startButton.Enabled = true;
                stopButton.Enabled = false;
                pathTextBox.Enabled = true;
                browseButton.Enabled = true;
                includeSubdirCheck.Enabled = true;
            };

            // Add controls to contentHolder in compact order
            contentHolder.Controls.Add(labelInputPath);
            contentHolder.Controls.Add(pathPanel);
            contentHolder.Controls.Add(optionsFlow);
            contentHolder.Controls.Add(contentLabel);
            contentHolder.Controls.Add(contentPanel);
            contentHolder.Controls.Add(buttonFlow);

            centerFlowPanel.Controls.Add(contentHolder);
            OperatingSystems.AddToMainContainer(centerFlowPanel);

            // Set references and focus
            pathTextBox.Focus();
        }

        private static void BrowseForFolder(TextBox pathTextBox)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder to monitor";
                try { folderDialog.SelectedPath = pathTextBox.Text; } catch { }
                folderDialog.ShowNewFolderButton = false;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }
    }
}
