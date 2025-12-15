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
        // --- Cancellation & State Flags ---
        private static bool _isRunning = false;
        private static bool _cancelRequested = false;

        private static void RunExperiment()
        {
            // 1. Handle Cancellation / Toggle Logic
            if (_isRunning)
            {
                // If already running, request stop
                _cancelRequested = true;
                _runButton.Text = "Stopping...";
                _runButton.Enabled = false; // Prevent double clicks while stopping
                return;
            }

            // 2. Initialize Experiment
            _isRunning = true;
            _cancelRequested = false;
            _runButton.Text = "Stop / إيقاف";
            _runButton.BackColor = Operating_Systems.ErrorColor; // Red for stop action

            // Read Inputs
            int iterations = (int)_iterationsBox.Value;
            int workloadMs = (int)_workloadBox.Value;
            bool useSleep = _testTypeBox.SelectedIndex == 0; // 0=Sleep, 1=Spin

            // Reset UI
            _resultsGrid.Rows.Clear();
            _testResults.Clear();
            _progressBar.Value = 0;
            _progressBar.Maximum = iterations;

            // Prepare Data Containers
            List<double> stopwatchSamples = new List<double>();
            List<double> dateTimeSamples = new List<double>();
            List<double> tickCountSamples = new List<double>();

            // 3. Main Measurement Loop
            for (int i = 0; i < iterations; i++)
            {
                // Check for cancel
                if (_cancelRequested) break;

                // --- A. Capture Start Times ---
                // 1. Stopwatch (High Resolution Hardware Timer)
                long swStart = Stopwatch.GetTimestamp();
                // 2. DateTime (System Clock)
                DateTime dtStart = DateTime.Now;
                // 3. TickCount (System Uptime in ms)
                int tcStart = Environment.TickCount;

                // --- B. Perform Work ---
                if (useSleep)
                {
                    // OS Scheduler puts thread to sleep
                    Thread.Sleep(workloadMs);
                }
                else
                {
                    // Busy Wait (CPU Spin) - keeps CPU active
                    Stopwatch spinSw = Stopwatch.StartNew();
                    while (spinSw.ElapsedMilliseconds < workloadMs) { /* Spin */ }
                    spinSw.Stop();
                }

                // --- C. Capture End Times ---
                long swEnd = Stopwatch.GetTimestamp();
                DateTime dtEnd = DateTime.Now;
                int tcEnd = Environment.TickCount;

                // --- D. Calculate Deltas ---
                // Convert ticks to milliseconds
                double swMs = (swEnd - swStart) * 1000.0 / Stopwatch.Frequency;
                double dtMs = (dtEnd - dtStart).TotalMilliseconds;
                double tcMs = (tcEnd - tcStart);

                stopwatchSamples.Add(swMs);
                dateTimeSamples.Add(dtMs);
                tickCountSamples.Add(tcMs);

                // --- E. Update UI (Every 5th iteration to save performance) ---
                if (i % 5 == 0 || i == iterations - 1)
                {
                    // Update Progress
                    _progressBar.Value = i + 1;
                    int percent = (int)((i + 1) / (float)iterations * 100);
                    _progressLabel.Text = $"{percent}%";

                    // Update Real-Time Chart Data
                    // We create a NEW list to avoid 'Collection Modified' errors in Paint event
                    var currentStats = new List<TestResult>
                    {
                        new TestResult { TimerName = "Stopwatch", MeanMs = stopwatchSamples.Average() },
                        new TestResult { TimerName = "DateTime", MeanMs = dateTimeSamples.Average() },
                        new TestResult { TimerName = "TickCount", MeanMs = tickCountSamples.Average() }
                    };

                    _testResults = currentStats;
                    _chartPanel.Invalidate(); // Trigger redraw

                    // Keep UI responsive
                    Application.DoEvents();
                }
            }

            // 4. Finalize Results
            // Add rows to the grid
            AddResultRow("Stopwatch (High Res)", stopwatchSamples, workloadMs);
            AddResultRow("DateTime (System Clock)", dateTimeSamples, workloadMs);
            AddResultRow("Environment.TickCount", tickCountSamples, workloadMs);

            // 5. Reset State
            _isRunning = false;
            _cancelRequested = false;
            _runButton.Text = "Run / تشغيل";
            _runButton.Enabled = true;
            _runButton.BackColor = Operating_Systems.AccentBlue;
            _progressLabel.Text = "Done";
        }
    }
}