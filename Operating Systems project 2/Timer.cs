using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class Timer
    {
        // UI references
        private static ComboBox _testTypeCombo;
        private static NumericUpDown _iterationsInput;
        private static NumericUpDown _durationInput;
        private static Button _runTestButton;
        private static Button _clearResultsButton;
        private static TextBox _resultsTextBox;
        private static Panel _chartPanel;
        private static Label _statusLabel;
        private static ProgressBar _progressBar;

        // Test data
        private static List<TestResult> _testResults = new List<TestResult>();
        private static volatile bool _testRunning = false;

        // Test result structure
        private class TestResult
        {
            public string TimerName { get; set; }
            public double MinMs { get; set; }
            public double MaxMs { get; set; }
            public double MeanMs { get; set; }
            public double StdDevMs { get; set; }
            public double DriftMs { get; set; }
            public long ResolutionTicks { get; set; }
            public List<double> Samples { get; set; } = new List<double>();
        }

        public static void ShowTimer(Operating_Systems OperatingSystems)
        {
            const int PanelWidth = 1110;
            int currentY = 20;
            const int Spacing = 12;
            const int LabelWidth = 120;
            const int ControlWidth = 200;

            // Main container
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Operating_Systems.Background,
                AutoScroll = true
            };

            // Header
            Label headerLabel = new Label
            {
                Text = "Timer Diagnostics & Benchmarking",
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(30, currentY),
                AutoSize = true
            };
            mainPanel.Controls.Add(headerLabel);
            currentY += 35;

            Label descLabel = new Label
            {
                Text = "Compare timing accuracy and resolution of .NET timer mechanisms",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                Location = new Point(30, currentY),
                AutoSize = true
            };
            mainPanel.Controls.Add(descLabel);
            currentY += 30;

            // Configuration section
            Panel configPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 20, 180),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            int configY = 15;
            int configX = 15;

            // Test Type
            Label testTypeLabel = new Label
            {
                Text = "Test Type:",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(configX, configY + 3),
                Size = new Size(LabelWidth, 20)
            };
            configPanel.Controls.Add(testTypeLabel);

            _testTypeCombo = new ComboBox
            {
                Location = new Point(configX + LabelWidth, configY),
                Size = new Size(ControlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                BackColor = Operating_Systems.Background,
                ForeColor = Operating_Systems.TextPrimary
            };
            _testTypeCombo.Items.AddRange(new object[] {
                "Resolution Test",
                "Accuracy Test (Short Intervals)",
                "Accuracy Test (Long Intervals)",
                "Drift Test",
                "Overhead Test"
            });
            _testTypeCombo.SelectedIndex = 0;
            configPanel.Controls.Add(_testTypeCombo);

            configY += 35;

            // Iterations
            Label iterLabel = new Label
            {
                Text = "Iterations:",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(configX, configY + 3),
                Size = new Size(LabelWidth, 20)
            };
            configPanel.Controls.Add(iterLabel);

            _iterationsInput = new NumericUpDown
            {
                Location = new Point(configX + LabelWidth, configY),
                Size = new Size(ControlWidth, 25),
                Minimum = 10,
                Maximum = 100000,
                Value = 1000,
                Font = new Font("Segoe UI", 9F),
                BackColor = Operating_Systems.Background,
                ForeColor = Operating_Systems.TextPrimary
            };
            configPanel.Controls.Add(_iterationsInput);

            configY += 35;

            // Duration
            Label durLabel = new Label
            {
                Text = "Interval (ms):",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(configX, configY + 3),
                Size = new Size(LabelWidth, 20)
            };
            configPanel.Controls.Add(durLabel);

            _durationInput = new NumericUpDown
            {
                Location = new Point(configX + LabelWidth, configY),
                Size = new Size(ControlWidth, 25),
                Minimum = 1,
                Maximum = 10000,
                Value = 100,
                Font = new Font("Segoe UI", 9F),
                BackColor = Operating_Systems.Background,
                ForeColor = Operating_Systems.TextPrimary
            };
            configPanel.Controls.Add(_durationInput);

            configY += 45;

            // Buttons
            _runTestButton = new Button
            {
                Text = "▶ Run Test",
                Location = new Point(configX, configY),
                Size = new Size(120, 35),
                BackColor = Operating_Systems.AccentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand
            };
            _runTestButton.FlatAppearance.BorderSize = 0;
            _runTestButton.Click += async (s, e) => await RunTest(OperatingSystems);
            configPanel.Controls.Add(_runTestButton);

            _clearResultsButton = new Button
            {
                Text = "Clear Results",
                Location = new Point(configX + 130, configY),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand
            };
            _clearResultsButton.FlatAppearance.BorderSize = 0;
            _clearResultsButton.Click += (s, e) => ClearResults();
            configPanel.Controls.Add(_clearResultsButton);

            mainPanel.Controls.Add(configPanel);
            currentY += configPanel.Height + Spacing;

            // Progress bar
            _progressBar = new ProgressBar
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 20, 20),
                Style = ProgressBarStyle.Continuous,
                Visible = false
            };
            mainPanel.Controls.Add(_progressBar);
            currentY += 25;

            // Status label
            _statusLabel = new Label
            {
                Text = "Ready to run tests",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                Location = new Point(0, currentY),
                AutoSize = true
            };
            mainPanel.Controls.Add(_statusLabel);
            currentY += 25;

            // Results section - split into text and chart
            int resultsWidth = (PanelWidth - 30) / 2;

            // Results text box
            Panel resultsPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(resultsWidth, 280),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label resultsHeader = new Label
            {
                Text = "Results Summary",
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(10, 8),
                AutoSize = true
            };
            resultsPanel.Controls.Add(resultsHeader);

            _resultsTextBox = new TextBox
            {
                Location = new Point(10, 35),
                Size = new Size(resultsWidth - 20, 235),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 8.5F),
                BackColor = Operating_Systems.Background,
                ForeColor = Operating_Systems.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            resultsPanel.Controls.Add(_resultsTextBox);
            mainPanel.Controls.Add(resultsPanel);

            // Chart panel
            _chartPanel = new Panel
            {
                Location = new Point(resultsWidth + 10, currentY),
                Size = new Size(resultsWidth, 280),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            _chartPanel.Paint += (s, e) => DrawChart(e.Graphics, _chartPanel.Width, _chartPanel.Height, OperatingSystems);

            Label chartHeader = new Label
            {
                Text = "Visual Comparison",
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(10, 8),
                AutoSize = true
            };
            _chartPanel.Controls.Add(chartHeader);
            mainPanel.Controls.Add(_chartPanel);

            OperatingSystems.AddToMainContainer(mainPanel);
        }

        private static async Task RunTest(Operating_Systems os)
        {
            if (_testRunning) return;

            _testRunning = true;
            _runTestButton.Enabled = false;
            _progressBar.Visible = true;
            _progressBar.Value = 0;
            _testResults.Clear();

            string testType = _testTypeCombo.SelectedItem.ToString();
            int iterations = (int)_iterationsInput.Value;
            int intervalMs = (int)_durationInput.Value;

            _statusLabel.Text = $"Running {testType}...";
            _statusLabel.ForeColor = Operating_Systems.AccentBlue;

            await Task.Run(() =>
            {
                switch (_testTypeCombo.SelectedIndex)
                {
                    case 0: // Resolution Test
                        RunResolutionTest(iterations);
                        break;
                    case 1: // Accuracy Test (Short)
                        RunAccuracyTest(iterations, intervalMs, true);
                        break;
                    case 2: // Accuracy Test (Long)
                        RunAccuracyTest(iterations, intervalMs, false);
                        break;
                    case 3: // Drift Test
                        RunDriftTest(iterations, intervalMs);
                        break;
                    case 4: // Overhead Test
                        RunOverheadTest(iterations);
                        break;
                }
            });

            DisplayResults();
            _chartPanel.Invalidate();

            _progressBar.Visible = false;
            _statusLabel.Text = "Test completed successfully";
            _statusLabel.ForeColor = Operating_Systems.AccentGreen;
            _runTestButton.Enabled = true;
            _testRunning = false;
        }

        private static void RunResolutionTest(int iterations)
        {
            // Test Stopwatch resolution
            var swResult = new TestResult { TimerName = "Stopwatch" };
            swResult.ResolutionTicks = Stopwatch.Frequency;

            for (int i = 0; i < iterations; i++)
            {
                long t1 = Stopwatch.GetTimestamp();
                long t2 = Stopwatch.GetTimestamp();
                double diff = (t2 - t1) * 1000.0 / Stopwatch.Frequency;
                swResult.Samples.Add(diff);
                UpdateProgress(i, iterations);
            }
            CalculateStats(swResult);
            _testResults.Add(swResult);

            // Test DateTime.Now resolution
            var dtResult = new TestResult { TimerName = "DateTime.Now" };
            dtResult.ResolutionTicks = TimeSpan.TicksPerMillisecond;

            for (int i = 0; i < iterations; i++)
            {
                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;
                double diff = (t2 - t1).TotalMilliseconds;
                dtResult.Samples.Add(diff);
            }
            CalculateStats(dtResult);
            _testResults.Add(dtResult);

            // Test Environment.TickCount resolution
            var tcResult = new TestResult { TimerName = "TickCount" };
            tcResult.ResolutionTicks = 10000; // ~1ms

            for (int i = 0; i < iterations; i++)
            {
                int t1 = Environment.TickCount;
                int t2 = Environment.TickCount;
                double diff = t2 - t1;
                tcResult.Samples.Add(diff);
            }
            CalculateStats(tcResult);
            _testResults.Add(tcResult);
        }

        private static void RunAccuracyTest(int iterations, int targetMs, bool shortInterval)
        {
            int actualInterval = shortInterval ? Math.Max(1, targetMs / 10) : targetMs;

            // Stopwatch accuracy
            var swResult = new TestResult { TimerName = "Stopwatch" };
            for (int i = 0; i < iterations; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                Thread.Sleep(actualInterval);
                sw.Stop();
                double measured = sw.Elapsed.TotalMilliseconds;
                swResult.Samples.Add(Math.Abs(measured - actualInterval));
                UpdateProgress(i, iterations);
            }
            CalculateStats(swResult);
            _testResults.Add(swResult);

            // DateTime.Now accuracy
            var dtResult = new TestResult { TimerName = "DateTime.Now" };
            for (int i = 0; i < iterations; i++)
            {
                DateTime start = DateTime.Now;
                Thread.Sleep(actualInterval);
                double measured = (DateTime.Now - start).TotalMilliseconds;
                dtResult.Samples.Add(Math.Abs(measured - actualInterval));
            }
            CalculateStats(dtResult);
            _testResults.Add(dtResult);

            // TickCount accuracy
            var tcResult = new TestResult { TimerName = "TickCount" };
            for (int i = 0; i < iterations; i++)
            {
                int start = Environment.TickCount;
                Thread.Sleep(actualInterval);
                double measured = Environment.TickCount - start;
                tcResult.Samples.Add(Math.Abs(measured - actualInterval));
            }
            CalculateStats(tcResult);
            _testResults.Add(tcResult);
        }

        private static void RunDriftTest(int iterations, int intervalMs)
        {
            // Measure cumulative drift over many intervals
            int actualInterval = Math.Max(10, intervalMs / 10);

            // Stopwatch drift
            var swResult = new TestResult { TimerName = "Stopwatch" };
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                long expected = (i + 1) * actualInterval;
                Thread.Sleep(actualInterval);
                double actual = sw.Elapsed.TotalMilliseconds;
                swResult.Samples.Add(Math.Abs(actual - expected));
                UpdateProgress(i, iterations);
            }
            CalculateStats(swResult);
            swResult.DriftMs = swResult.Samples.Last();
            _testResults.Add(swResult);

            // DateTime drift
            var dtResult = new TestResult { TimerName = "DateTime.Now" };
            DateTime dtStart = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                double expected = (i + 1) * actualInterval;
                Thread.Sleep(actualInterval);
                double actual = (DateTime.Now - dtStart).TotalMilliseconds;
                dtResult.Samples.Add(Math.Abs(actual - expected));
            }
            CalculateStats(dtResult);
            dtResult.DriftMs = dtResult.Samples.Last();
            _testResults.Add(dtResult);

            // TickCount drift
            var tcResult = new TestResult { TimerName = "TickCount" };
            int tcStart = Environment.TickCount;
            for (int i = 0; i < iterations; i++)
            {
                double expected = (i + 1) * actualInterval;
                Thread.Sleep(actualInterval);
                double actual = Environment.TickCount - tcStart;
                tcResult.Samples.Add(Math.Abs(actual - expected));
            }
            CalculateStats(tcResult);
            tcResult.DriftMs = tcResult.Samples.Last();
            _testResults.Add(tcResult);
        }

        private static void RunOverheadTest(int iterations)
        {
            // Measure the overhead of calling each timer

            // Stopwatch overhead
            var swResult = new TestResult { TimerName = "Stopwatch" };
            Stopwatch master = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                long dummy = Stopwatch.GetTimestamp();
                UpdateProgress(i, iterations);
            }
            master.Stop();
            double swOverhead = master.Elapsed.TotalMilliseconds / iterations;
            swResult.Samples.Add(swOverhead);
            swResult.MeanMs = swOverhead;
            _testResults.Add(swResult);

            // DateTime overhead
            var dtResult = new TestResult { TimerName = "DateTime.Now" };
            master.Restart();
            for (int i = 0; i < iterations; i++)
            {
                DateTime dummy = DateTime.Now;
            }
            master.Stop();
            double dtOverhead = master.Elapsed.TotalMilliseconds / iterations;
            dtResult.Samples.Add(dtOverhead);
            dtResult.MeanMs = dtOverhead;
            _testResults.Add(dtResult);

            // TickCount overhead
            var tcResult = new TestResult { TimerName = "TickCount" };
            master.Restart();
            for (int i = 0; i < iterations; i++)
            {
                int dummy = Environment.TickCount;
            }
            master.Stop();
            double tcOverhead = master.Elapsed.TotalMilliseconds / iterations;
            tcResult.Samples.Add(tcOverhead);
            tcResult.MeanMs = tcOverhead;
            _testResults.Add(tcResult);
        }

        private static void CalculateStats(TestResult result)
        {
            if (result.Samples.Count == 0) return;

            result.MinMs = result.Samples.Min();
            result.MaxMs = result.Samples.Max();
            result.MeanMs = result.Samples.Average();

            // Standard deviation
            double sumSquares = result.Samples.Sum(x => Math.Pow(x - result.MeanMs, 2));
            result.StdDevMs = Math.Sqrt(sumSquares / result.Samples.Count);
        }

        private static void DisplayResults()
        {
            if (_testResults.Count == 0)
            {
                _resultsTextBox.Text = "No test results available.";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("  TIMER DIAGNOSTICS RESULTS");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine();

            foreach (var result in _testResults)
            {
                sb.AppendLine($"Timer: {result.TimerName}");
                sb.AppendLine(new string('-', 39));

                if (result.ResolutionTicks > 0)
                    sb.AppendLine($"  Resolution: {result.ResolutionTicks:N0} ticks");

                sb.AppendLine($"  Min:        {result.MinMs:F6} ms");
                sb.AppendLine($"  Max:        {result.MaxMs:F6} ms");
                sb.AppendLine($"  Mean:       {result.MeanMs:F6} ms");
                sb.AppendLine($"  Std Dev:    {result.StdDevMs:F6} ms");

                if (result.DriftMs > 0)
                    sb.AppendLine($"  Total Drift: {result.DriftMs:F3} ms");

                sb.AppendLine($"  Samples:    {result.Samples.Count:N0}");
                sb.AppendLine();
            }

            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"Test completed: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            _resultsTextBox.Text = sb.ToString();
        }

        private static void DrawChart(Graphics g, int width, int height, Operating_Systems os)
        {
            if (_testResults.Count == 0) return;

            int chartTop = 40;
            int chartBottom = height - 30;
            int chartLeft = 60;
            int chartRight = width - 20;
            int chartHeight = chartBottom - chartTop;
            int chartWidth = chartRight - chartLeft;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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
            int barWidth = chartWidth / (_testResults.Count * 2);
            Color[] colors = { Operating_Systems.AccentGreen, Operating_Systems.AccentBlue, Color.FromArgb(255, 193, 7) };

            for (int i = 0; i < _testResults.Count; i++)
            {
                var result = _testResults[i];
                int barHeight = (int)(result.MeanMs / maxValue * chartHeight);
                int x = chartLeft + (i * 2 + 1) * barWidth;
                int y = chartBottom - barHeight;

                using (SolidBrush brush = new SolidBrush(colors[i % colors.Length]))
                {
                    g.FillRectangle(brush, x, y, barWidth, barHeight);
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

        private static void UpdateProgress(int current, int total)
        {
            if (_progressBar.InvokeRequired)
            {
                _progressBar.Invoke(new Action(() =>
                {
                    _progressBar.Value = Math.Min(100, (int)(current * 100.0 / total));
                }));
            }
            else
            {
                _progressBar.Value = Math.Min(100, (int)(current * 100.0 / total));
            }
        }

        private static void ClearResults()
        {
            _testResults.Clear();
            _resultsTextBox.Text = string.Empty;
            _chartPanel.Invalidate();
            _statusLabel.Text = "Results cleared. Ready to run tests.";
        }
    }
}