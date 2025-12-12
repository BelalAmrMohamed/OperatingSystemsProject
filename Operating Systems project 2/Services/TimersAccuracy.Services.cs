using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class TimersAccuracy
    {
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

        // Helpers
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
