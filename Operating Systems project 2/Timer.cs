using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq; // Added for math (Average, Sum)
using System.Threading;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class Timer
    {
        // UI Controls
        private static NumericUpDown _iterationsBox;
        private static NumericUpDown _workloadBox;
        private static ComboBox _testTypeBox;
        private static Button _runButton;
        private static Panel _chartPanel;            // custom drawn chart area
        private static DataGridView _resultsGrid;
        private static ProgressBar _progressBar;    // progress bar
        private static Label _progressLabel;        // shows percent

        // reference to OS form for colors
        private static Operating_Systems _osRef;

        // Data storage for results
        private struct TimerResult
        {
            public string Name;
            public List<double> Samples; // Duration in ms for each iteration
        }

        // simplified structure used by DrawChart
        private class TestResult
        {
            public string TimerName { get; set; }
            public double MeanMs { get; set; }
        }
        private static List<TestResult> _testResults = new List<TestResult>();

        public static void ShowTimer(Operating_Systems OperatingSystems)
        {
            _osRef = OperatingSystems;

            // 1. Layout Constants
            const int PanelWidth = 1104;
            const int ContainerHeight = 482;
            int currentY = 40; // Top margin reserved


            // 3. Configuration Row (Top)
            Panel configPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 50),
                BackColor = Operating_Systems.PanelColor
            };

            // Controls for Config
            Label lblType = MakeLabel("Test Type:", 10, 15);
            _testTypeBox = new ComboBox { Location = new Point(90, 12), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Operating_Systems.PanelColor, ForeColor = Operating_Systems.TextPrimary, FlatStyle = FlatStyle.Flat };
            _testTypeBox.Items.AddRange(new object[] { "Thread.Sleep", "Busy Wait (Spin)" });
            _testTypeBox.SelectedIndex = 0;

            Label lblIter = MakeLabel("Iterations:", 250, 15);
            _iterationsBox = MakeNumeric(330, 12, 100, 1, 10000); // Default 100

            Label lblWork = MakeLabel("Work (ms):", 420, 15);
            _workloadBox = MakeNumeric(500, 12, 20, 1, 5000); // Default 20ms

            _runButton = new Button
            {
                Text = "Run Diagnostics",
                Location = new Point(620, 8),
                Size = new Size(160, 34),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F),
                Cursor = Cursors.Hand
            };
            _runButton.FlatAppearance.BorderSize = 0;
            _runButton.Click += (s, e) => RunExperiment();

            // ProgressBar + label
            _progressBar = new ProgressBar
            {
                Location = new Point(800, 14),
                Size = new Size(200, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            _progressLabel = new Label
            {
                Text = "",
                Location = new Point(1008, 12),
                AutoSize = true,
                ForeColor = Operating_Systems.TextSecondary,
                Font = new Font("Segoe UI", 8F)
            };

            configPanel.Controls.Add(lblType);
            configPanel.Controls.Add(_testTypeBox);
            configPanel.Controls.Add(lblIter);
            configPanel.Controls.Add(_iterationsBox);
            configPanel.Controls.Add(lblWork);
            configPanel.Controls.Add(_workloadBox);
            configPanel.Controls.Add(_runButton);
            configPanel.Controls.Add(_progressBar);
            configPanel.Controls.Add(_progressLabel);

            currentY += configPanel.Height + 12;

            // 4. Custom Chart Section (uses provided DrawChart method)
            _chartPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 220),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.None
            };
            // Smooth repaint
            _chartPanel.Paint += (s, e) => DrawChart(e.Graphics, _chartPanel.Width, _chartPanel.Height, _osRef);
            _chartPanel.Resize += (s, e) => _chartPanel.Invalidate();

            currentY += _chartPanel.Height + 10;

            // 5. Data Grid (Statistics Table)
            _resultsGrid = new DataGridView
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 180),
                BackgroundColor = Operating_Systems.Background,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            // Dark theme for headers / rows
            _resultsGrid.EnableHeadersVisualStyles = false;
            _resultsGrid.ColumnHeadersDefaultCellStyle.BackColor = Operating_Systems.PanelColor;
            _resultsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Operating_Systems.TextPrimary;
            _resultsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
            _resultsGrid.DefaultCellStyle.BackColor = Operating_Systems.Background;
            _resultsGrid.DefaultCellStyle.ForeColor = Operating_Systems.TextPrimary;
            _resultsGrid.DefaultCellStyle.SelectionBackColor = Operating_Systems.AccentBlue;
            _resultsGrid.GridColor = Color.FromArgb(60, 60, 60);

            // Define Columns
            _resultsGrid.Columns.Add("Source", "Timer Source");
            _resultsGrid.Columns.Add("Min", "Min (ms)");
            _resultsGrid.Columns.Add("Max", "Max (ms)");
            _resultsGrid.Columns.Add("Avg", "Mean (ms)");
            _resultsGrid.Columns.Add("StdDev", "StdDev (ms)");
            _resultsGrid.Columns.Add("Drift", "Total Drift (ms)");

            // Add controls to holder
            OperatingSystems.AddToMainContainer(configPanel);
            OperatingSystems.AddToMainContainer(_chartPanel);
            OperatingSystems.AddToMainContainer(_resultsGrid);
        }

        // --- EXPERIMENT LOGIC ---

        private static void RunExperiment()
        {
            _runButton.Enabled = false;
            _runButton.Text = "Running 0%";
            Application.DoEvents(); // Force UI update

            int iterations = (int)_iterationsBox.Value;
            int workloadMs = (int)_workloadBox.Value;
            bool useSleep = _testTypeBox.SelectedIndex == 0;

            // Prepare storage
            var swSamples = new List<double>();
            var dtSamples = new List<double>();
            var tcSamples = new List<double>();

            // Clear previous results
            _testResults.Clear();
            _resultsGrid.Rows.Clear();
            _chartPanel.Invalidate();

            _progressBar.Maximum = Math.Max(1, iterations);
            _progressBar.Value = 0;
            _progressLabel.Text = "0%";

            // --- THE LOOP ---
            for (int i = 0; i < iterations; i++)
            {
                long swStart = Stopwatch.GetTimestamp();
                DateTime dtStart = DateTime.Now;
                int tcStart = Environment.TickCount;

                // Simulate Work
                if (useSleep)
                {
                    Thread.Sleep(workloadMs);
                }
                else
                {
                    // Busy wait (spin)
                    long spinUntil = Stopwatch.GetTimestamp() + (long)(workloadMs * Stopwatch.Frequency / 1000.0);
                    while (Stopwatch.GetTimestamp() < spinUntil) { /* Spin */ }
                }

                long swEnd = Stopwatch.GetTimestamp();
                DateTime dtEnd = DateTime.Now;
                int tcEnd = Environment.TickCount;

                // Calculate Deltas (ms)
                double swDelta = (swEnd - swStart) * 1000.0 / Stopwatch.Frequency;
                double dtDelta = (dtEnd - dtStart).TotalMilliseconds;
                double tcDelta = (tcEnd - tcStart); // TickCount is essentially milliseconds

                swSamples.Add(swDelta);
                dtSamples.Add(dtDelta);
                tcSamples.Add(tcDelta);

                // Update progress
                try
                {
                    _progressBar.Value = Math.Min(_progressBar.Maximum, i + 1);
                }
                catch { /* in case of cross-thread but we're on UI thread */ }
                int pct = (int)(100.0 * (i + 1) / iterations);
                _runButton.Text = $"Running {pct}%";
                _progressLabel.Text = $"{pct}%";

                // Update Chart live (every 5th frame to save UI perf)
                if ((i % 5 == 0) || i == iterations - 1)
                {
                    // temporary partial mean values to show evolving chart
                    var partialResults = new List<TestResult>
                    {
                        new TestResult { TimerName = "Stopwatch", MeanMs = swSamples.Count>0 ? swSamples.Average() : 0 },
                        new TestResult { TimerName = "DateTime", MeanMs = dtSamples.Count>0 ? dtSamples.Average() : 0 },
                        new TestResult { TimerName = "TickCount", MeanMs = tcSamples.Count>0 ? tcSamples.Average() : 0 }
                    };
                    // swap into main test result list for DrawChart to read
                    _testResults = partialResults;
                    _chartPanel.Invalidate(); // triggers Paint (DrawChart)
                    Application.DoEvents();
                }
            }

            // Compute final _testResults (means)
            _testResults = new List<TestResult>
            {
                new TestResult { TimerName = "Stopwatch", MeanMs = swSamples.Average() },
                new TestResult { TimerName = "DateTime", MeanMs = dtSamples.Average() },
                new TestResult { TimerName = "TickCount", MeanMs = tcSamples.Average() }
            };

            // Fill missing chart points - request repaint
            _chartPanel.Invalidate();

            // Compute and Display Stats
            AddResultRow("Sys.Diag.Stopwatch", swSamples, iterations * workloadMs);
            AddResultRow("DateTime.Now", dtSamples, iterations * workloadMs);
            AddResultRow("Env.TickCount", tcSamples, iterations * workloadMs);

            // Finalize progress
            _progressBar.Value = _progressBar.Maximum;
            _runButton.Enabled = true;
            _runButton.Text = "Run Diagnostics";
            _progressLabel.Text = "Done";
        }

        // --- HELPERS ---

        private static void AddResultRow(string name, List<double> data, double expectedTotal)
        {
            double min = data.Min();
            double max = data.Max();
            double avg = data.Average();
            double totalActual = data.Sum();
            double drift = totalActual - expectedTotal; // Positive means run took longer than expected

            // Standard Deviation (sample)
            double stdDev = 0;
            if (data.Count > 1)
            {
                double sumSquares = data.Sum(d => Math.Pow(d - avg, 2));
                stdDev = Math.Sqrt(sumSquares / (data.Count - 1));
            }

            _resultsGrid.Rows.Add(
                name,
                min.ToString("F3"),
                max.ToString("F3"),
                avg.ToString("F3"),
                stdDev.ToString("F3"),
                drift.ToString("F1")
            );
        }

        private static Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Operating_Systems.TextPrimary,
                Font = new Font("Segoe UI", 9F)
            };
        }

        private static NumericUpDown MakeNumeric(int x, int y, int val, int min, int max)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Width = 70,
                Minimum = min,
                Maximum = max,
                Value = val,
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                Font = new Font("Segoe UI", 9F)
            };
        }

        // --- Custom chart drawing (uses your provided method, adapted to read _testResults) ---
        private static void DrawChart(Graphics g, int width, int height, Operating_Systems os)
        {
            if (_testResults == null || _testResults.Count == 0) return;

            int chartTop = 40;
            int chartBottom = height - 30;
            int chartLeft = 60;
            int chartRight = width - 20;
            int chartHeight = chartBottom - chartTop;
            int chartWidth = chartRight - chartLeft;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw axes
            using (Pen axisPen = new Pen(Operating_Systems.TextSecondary, 1))
            {
                g.DrawLine(axisPen, chartLeft, chartBottom, chartRight, chartBottom); // X-axis
                g.DrawLine(axisPen, chartLeft, chartTop, chartLeft, chartBottom);     // Y-axis
            }

            // Find max value for scaling
            double maxValue = _testResults.Max(r => r.MeanMs);
            if (maxValue == 0) maxValue = 1;

            // Bar chart
            int barWidth = Math.Max(6, chartWidth / (_testResults.Count * 3));
            Color[] colors = { Operating_Systems.AccentGreen, Operating_Systems.AccentBlue, Operating_Systems.ErrorColor };

            for (int i = 0; i < _testResults.Count; i++)
            {
                var result = _testResults[i];
                int barHeight = (int)(result.MeanMs / maxValue * chartHeight);
                int x = chartLeft + (i * 2 + 1) * barWidth;
                int y = chartBottom - barHeight;

                using (SolidBrush brush = new SolidBrush(colors[i % colors.Length]))
                {
                    // slightly rounded rectangle
                    var rect = new Rectangle(x, y, barWidth, barHeight);
                    g.FillRectangle(brush, rect);
                }

                // Label
                using (Font font = new Font("Segoe UI", 7F))
                using (SolidBrush textBrush = new SolidBrush(Operating_Systems.TextPrimary))
                {
                    string label = result.TimerName;
                    SizeF labelSize = g.MeasureString(label, font);
                    g.DrawString(label, font, textBrush, x + barWidth / 2 - labelSize.Width / 2, chartBottom + 5);

                    string value = $"{result.MeanMs:F3}ms";
                    SizeF valueSize = g.MeasureString(value, font);
                    g.DrawString(value, font, textBrush, x + barWidth / 2 - valueSize.Width / 2, y - 15);
                }
            }

            // Y-axis labels
            using (Font font = new Font("Segoe UI", 7F))
            using (SolidBrush textBrush = new SolidBrush(Operating_Systems.TextSecondary))
            {
                for (int i = 0; i <= 5; i++)
                {
                    double value = maxValue * i / 5.0;
                    int y = chartBottom - (int)(chartHeight * i / 5.0);
                    g.DrawString($"{value:F2}", font, textBrush, 5, y - 7);
                }
            }

            // Title
            using (Font titleFont = new Font("Segoe UI Semibold", 9F))
            using (SolidBrush titleBrush = new SolidBrush(Operating_Systems.TextPrimary))
            {
                g.DrawString("Mean Time Comparison", titleFont, titleBrush, chartLeft, 10);
            }
        }
    }
}