using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class TimersAccuracy
    {
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
    }
}