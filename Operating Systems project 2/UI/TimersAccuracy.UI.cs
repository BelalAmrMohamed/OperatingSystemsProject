using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class TimersAccuracy
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

        public static void ShowTimer(Operating_Systems OS)
        {
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
            _chartPanel.Paint += (s, e) => DrawChart(e.Graphics, _chartPanel.Width, _chartPanel.Height, OS);
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
            OS.AddToMainContainer(configPanel);
            OS.AddToMainContainer(_chartPanel);
            OS.AddToMainContainer(_resultsGrid);
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
    }
}