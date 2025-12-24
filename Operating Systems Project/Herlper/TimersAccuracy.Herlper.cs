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
        // Simple DTO for the chart
        private class TestResult
        {
            public string TimerName { get; set; }
            public double MeanMs { get; set; }
        }

        // Shared list for Chart data
        private static List<TestResult> _testResults = new List<TestResult>();

        // Helper: Formats numbers and adds a row to the grid
        private static void AddResultRow(string name, List<double> data, double expectedMs)
        {
            if (data == null || data.Count == 0) return;

            double min = data.Min();
            double max = data.Max();
            double avg = data.Average();
            double diff = avg - expectedMs; // How far off the average was from target

            _resultsGrid.Rows.Add(
                name,
                min.ToString("F3"),
                max.ToString("F3"),
                avg.ToString("F3"),
                (diff > 0 ? "+" : "") + diff.ToString("F3") // Show +/- sign
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

        // --- Chart Drawing Logic ---
        private static void DrawChart(Graphics g, int width, int height, Operating_Systems os)
        {
            // Safety check
            if (_testResults == null || _testResults.Count == 0) return;

            int chartTop = 40;
            int chartBottom = height - 30;
            int chartLeft = 60;
            int chartRight = width - 20;
            int chartHeight = chartBottom - chartTop;
            int chartWidth = chartRight - chartLeft;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Draw Axes
            using (Pen axisPen = new Pen(Operating_Systems.TextSecondary, 1))
            {
                g.DrawLine(axisPen, chartLeft, chartBottom, chartRight, chartBottom); // X-axis
                g.DrawLine(axisPen, chartLeft, chartTop, chartLeft, chartBottom);     // Y-axis
            }

            // 2. Determine Scale (Max Value)
            double maxValue = _testResults.Max(r => r.MeanMs);
            if (maxValue <= 0) maxValue = 1; // Prevent divide by zero
            maxValue = maxValue * 1.1; // Add 10% headroom

            // 3. Draw Bars
            int barWidth = Math.Max(20, chartWidth / (_testResults.Count * 2));
            int gap = barWidth;

            // Colors: Stopwatch (Green/Precise), DateTime (Blue/Okay), TickCount (Red/Coarse)
            Color[] colors = { Operating_Systems.AccentGreen, Operating_Systems.AccentBlue, Operating_Systems.ErrorColor };

            for (int i = 0; i < _testResults.Count; i++)
            {
                var result = _testResults[i];

                // Calculate height relative to max value
                int barHeight = (int)((result.MeanMs / maxValue) * chartHeight);

                // Coordinates
                int x = chartLeft + gap + (i * (barWidth + gap));
                int y = chartBottom - barHeight;

                using (SolidBrush brush = new SolidBrush(colors[i % colors.Length]))
                {
                    g.FillRectangle(brush, x, y, barWidth, barHeight);
                }

                // Text Labels
                using (Font font = new Font("Segoe UI", 8F))
                using (SolidBrush textBrush = new SolidBrush(Operating_Systems.TextPrimary))
                {
                    // Draw Name below bar
                    string label = result.TimerName;
                    SizeF labelSize = g.MeasureString(label, font);
                    g.DrawString(label, font, textBrush, x + (barWidth - labelSize.Width) / 2, chartBottom + 5);

                    // Draw Value above bar
                    string valueText = $"{result.MeanMs:F2} ms";
                    SizeF valueSize = g.MeasureString(valueText, font);
                    g.DrawString(valueText, font, textBrush, x + (barWidth - valueSize.Width) / 2, y - 20);
                }
            }

            // 4. Title
            using (Font titleFont = new Font("Segoe UI Semibold", 10F))
            using (SolidBrush titleBrush = new SolidBrush(Operating_Systems.TextPrimary))
            {
                g.DrawString("Average Accuracy Comparison / متوسط الدقة", titleFont, titleBrush, chartLeft, 10);
            }
        }
    }
}