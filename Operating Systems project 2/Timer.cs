using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class Timer
    {
        // UI references
        private static Label _timerLabel;
        private static ListBox _lapListBox;
        private static Label _lapCountLabel;
        private static Button _startButton;
        private static Button _pauseButton;
        private static Button _resetButton;
        private static Button _lapButton;

        // Timing
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static readonly System.Windows.Forms.Timer _uiTimer = new System.Windows.Forms.Timer();
        private static volatile bool _isRunning = false;
        private static readonly List<TimeSpan> _laps = new List<TimeSpan>();

        public static void ShowTimer(Operating_Systems OperatingSystems)
        {            
            // Stop UI timer if previously running
            _uiTimer.Stop();
            _uiTimer.Tick -= UiTimer_Tick;
            _uiTimer.Interval = 25; // ~40 FPS updates
            _uiTimer.Tick += UiTimer_Tick;

            // Layout constants (compact)
            const int PanelWidth = 1104;
            const int ContainerHeight = 482;
            const int VerticalSpacing = 8;
            int currentY = 0;

            // Centering flow + fixed content holder
            FlowLayoutPanel centerFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Operating_Systems.Background,
                Padding = new Padding(0),
                AutoScroll = false
            };

            Panel contentHolder = new Panel
            {
                Size = new Size(PanelWidth, ContainerHeight),
                BackColor = Operating_Systems.Background,
                Margin = new Padding(0)
            };
            
            currentY += 40;

            // Timer display (compact large number)
            Panel timerPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, 96),
                BackColor = Operating_Systems.Background,
                BorderStyle = BorderStyle.None
            };

            _timerLabel = new Label
            {
                Name = "timerLabel",
                Text = "00:00:00.000",
                Font = new Font("Consolas", 36F, FontStyle.Bold),
                ForeColor = Operating_Systems.AccentGreen,
                AutoSize = false,
                Size = new Size(timerPanel.Width, timerPanel.Height),
                TextAlign = ContentAlignment.MiddleCenter
            };

            timerPanel.Controls.Add(_timerLabel);
            currentY += timerPanel.Height + VerticalSpacing;

            // Buttons row (compact)
            FlowLayoutPanel buttonsRow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, 48),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
            };

            _startButton = MakeButton("▶ Start", 120, Operating_Systems.AccentGreen, 10, fontSize: 10.5f);
            _pauseButton = MakeButton("⏸ Pause", 120, Color.FromArgb(255, 193, 7), 0, fontSize: 10.5f);
            _resetButton = MakeButton("⏹ Reset", 120, Operating_Systems.ErrorColor, 0, fontSize: 10.5f);
            _lapButton = MakeButton("📍 Lap", 120, Operating_Systems.AccentBlue, 0, fontSize: 10.5f);

            _pauseButton.Enabled = false;
            _resetButton.Enabled = false;
            _lapButton.Enabled = false;

            _startButton.Click += (s, e) => Start();
            _pauseButton.Click += (s, e) => Pause();
            _resetButton.Click += (s, e) => Reset();
            _lapButton.Click += (s, e) => RecordLap();

            buttonsRow.Controls.Add(_startButton);
            buttonsRow.Controls.Add(_pauseButton);
            buttonsRow.Controls.Add(_resetButton);
            buttonsRow.Controls.Add(_lapButton);

            currentY += buttonsRow.Height + (VerticalSpacing * 2);

            // Laps header (compact)
            Label lapsHeader = new Label
            {
                Text = "Laps",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };

            _lapCountLabel = new Label
            {
                Text = "No laps recorded",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(80, currentY + 2)
            };

            currentY += lapsHeader.Height + 6;

            // Compute remaining height for lap list and actions
            int bottomRowHeight = 46;
            int lapAreaHeight = contentHolder.Height - currentY - bottomRowHeight - (VerticalSpacing * 2);
            if (lapAreaHeight < 120) lapAreaHeight = 120;

            Panel lapPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, lapAreaHeight),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            _lapListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            lapPanel.Controls.Add(_lapListBox);

            currentY += lapPanel.Height + VerticalSpacing;

            // Lap actions row
            FlowLayoutPanel lapActions = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, bottomRowHeight),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
            };

            Button clearLapsBtn = MakeButton("Clear Laps", 110, Color.FromArgb(90, 90, 90), 0, fontSize: 9f);
            Button copyBtn = MakeButton("📋 Copy Time", 120, Color.FromArgb(90, 90, 90), 8, fontSize: 9f);
            Button exportBtn = MakeButton("💾 Export Laps", 120, Color.FromArgb(90, 90, 90), 8, fontSize: 9f);

            clearLapsBtn.Click += (s, e) => ClearLaps();
            copyBtn.Click += (s, e) => CopyCurrentTime();
            exportBtn.Click += (s, e) => ExportLaps();

            lapActions.Controls.Add(clearLapsBtn);
            lapActions.Controls.Add(copyBtn);
            lapActions.Controls.Add(exportBtn);

            // Add everything into content holder in order
            contentHolder.Controls.Add(timerPanel);
            contentHolder.Controls.Add(buttonsRow);
            contentHolder.Controls.Add(lapsHeader);
            contentHolder.Controls.Add(_lapCountLabel);
            contentHolder.Controls.Add(lapPanel);
            contentHolder.Controls.Add(lapActions);

            centerFlow.Controls.Add(contentHolder);
            OperatingSystems.AddToMainContainer(centerFlow);

            // safety: make sure UI timer is stopped until user starts
            _uiTimer.Stop();
        }

        // helper to create consistent styled buttons
        private static Button MakeButton(string text, int width, Color back, int leftMargin = 0, float fontSize = 10f)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(width, 40),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", fontSize),
                Cursor = Cursors.Hand,
                Margin = new Padding(leftMargin, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // UI timer tick — updates label from the stopwatch (runs on UI thread)
        private static void UiTimer_Tick(object sender, EventArgs e)
        {
            if (_timerLabel == null || _timerLabel.IsDisposed) return;
            _timerLabel.Text = FormatTimeSpan(_stopwatch.Elapsed);
        }

        // Controls behavior
        private static void Start()
        {
            if (_isRunning) return;

            _stopwatch.Start();
            _uiTimer.Start();
            _isRunning = true;
            UpdateButtons();
            UpdateTimerColor();
        }

        private static void Pause()
        {
            if (!_isRunning) return;

            _stopwatch.Stop();
            _uiTimer.Stop();
            _isRunning = false;
            UpdateButtons();
            UpdateTimerColor();
        }

        private static void Reset()
        {
            _stopwatch.Reset();
            _uiTimer.Stop();
            _isRunning = false;
            _timerLabel.Text = "00:00:00.000";
            ClearLaps();
            UpdateButtons();
            UpdateTimerColor();
        }

        private static void RecordLap()
        {
            if (!_stopwatch.IsRunning) return;

            var now = _stopwatch.Elapsed;
            TimeSpan split = (_laps.Count == 0) ? now : now - _laps[_laps.Count - 1];
            _laps.Add(now);

            string entry = $"Lap {_laps.Count:D2}: {FormatTimeSpan(now)}  (Split: {FormatTimeSpan(split)})";
            _lapListBox.Items.Add(entry);
            _lapListBox.SelectedIndex = _lapListBox.Items.Count - 1;
            UpdateLapCount();
        }

        private static void ClearLaps()
        {
            _laps.Clear();
            _lapListBox.Items.Clear();
            UpdateLapCount();
        }

        private static void UpdateLapCount()
        {
            if (_lapCountLabel == null) return;

            if (_laps.Count == 0) _lapCountLabel.Text = "No laps recorded";
            else if (_laps.Count == 1) _lapCountLabel.Text = "1 lap recorded";
            else _lapCountLabel.Text = $"{_laps.Count} laps recorded";
        }

        private static void CopyCurrentTime()
        {
            string now = (_timerLabel?.Text) ?? "00:00:00.000";
            Clipboard.SetText(now);
            MessageBox.Show($"Current time copied to clipboard:\n{now}", "Copy Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void ExportLaps()
        {
            if (_laps.Count == 0)
            {
                MessageBox.Show("No lap times to export.", "Export Laps", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Export Lap Times";
                dlg.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                dlg.FileName = $"Stopwatch_Laps_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Stopwatch Lap Times");
                    sb.AppendLine($"Exported: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine(new string('-', 60));
                    for (int i = 0; i < _laps.Count; i++)
                    {
                        TimeSpan split = i == 0 ? _laps[i] : _laps[i] - _laps[i - 1];
                        sb.AppendLine($"Lap {i + 1:D2}: Total={FormatTimeSpan(_laps[i])}  Split={FormatTimeSpan(split)}");
                    }
                    sb.AppendLine(new string('-', 60));
                    sb.AppendLine($"Total Laps: {_laps.Count}");
                    sb.AppendLine($"Final Time: {FormatTimeSpan(_laps[_laps.Count - 1])}");

                    File.WriteAllText(dlg.FileName, sb.ToString());
                    MessageBox.Show($"Lap times exported successfully to:\n{dlg.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export lap times:\n{ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // helper to set button enabled states and colors
        private static void UpdateButtons()
        {
            if (_startButton != null) _startButton.Enabled = !_isRunning;
            if (_pauseButton != null) _pauseButton.Enabled = _isRunning;
            if (_resetButton != null) _resetButton.Enabled = !_isRunning && _stopwatch.Elapsed.TotalMilliseconds > 0;
            if (_lapButton != null) _lapButton.Enabled = _isRunning;
        }

        private static void UpdateTimerColor()
        {
            if (_timerLabel == null) return;
            _timerLabel.ForeColor = _isRunning ? Operating_Systems.AccentGreen : Operating_Systems.AccentBlue;
        }

        private static string FormatTimeSpan(TimeSpan t)
        {
            return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}.{t.Milliseconds:D3}";
        }
    }
}