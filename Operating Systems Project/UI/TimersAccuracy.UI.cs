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
        private static Panel _chartPanel;
        private static DataGridView _resultsGrid;
        private static ProgressBar _progressBar;
        private static Label _progressLabel;
       
        public static void ShowTimer(Operating_Systems OS)
        {
            // 1. Layout Constants
            const int PanelWidth = 1104;
            int currentY = 0; // سنبدأ من الأعلى ونضع العنوان أولًا            

            Label titleLabel = new Label
            {
                Text = "Timers Accuracy: ",
                Location = new Point(20, 8),
                AutoSize = true,
                ForeColor = Operating_Systems.TextPrimary,
                Font = new Font("Segoe UI Semibold", 13F)
            };

            // وصف موجز أصغر بحجم خط أقل - مفيد للمستخدمين الجدد
            Label descLabel = new Label
            {
                Text = "Compare Stopwatch, DateTime.Now and Environment.TickCount accuracy",
                Location = new Point(180, 15),
                AutoSize = true,
                ForeColor = Operating_Systems.TextSecondary,
                Font = new Font("Segoe UI", 8F)
            };

            // ريثما نضيف الهيدر، نحدّث currentY ليبدأ الصف التالي بعد الهيدر
            currentY += titleLabel.Height + descLabel.Height + 6; // مسافة بسيطة أسفل الهيدر

            // ====== Configuration Row (Top) ======
            Panel configPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 50),
                BackColor = Operating_Systems.PanelColor
            };

            // Controls for Config - خطوط موحّدة لمظهر أنيق
            Label lblType = MakeLabel("Test Type / نوع الاختبار:", 10, 15);
            lblType.Font = new Font("Segoe UI", 9F);

            _testTypeBox = new ComboBox
            {
                Location = new Point(140, 12),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };
            _testTypeBox.Items.AddRange(new object[] { "Thread.Sleep", "Busy Wait (Spin)" });
            _testTypeBox.SelectedIndex = 0;

            Label lblIter = MakeLabel("Iterations / التكرار:", 300, 15);
            lblIter.Font = new Font("Segoe UI", 9F);
            _iterationsBox = MakeNumeric(410, 12, 100, 1, 10000);

            Label lblWork = MakeLabel("Work (ms) / المدة:", 500, 15);
            lblWork.Font = new Font("Segoe UI", 9F);
            _workloadBox = MakeNumeric(610, 12, 20, 1, 5000);

            _runButton = new Button
            {
                Text = "Run / تشغيل",
                Location = new Point(700, 8),
                Size = new Size(130, 34),
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
                Location = new Point(850, 14),
                Size = new Size(200, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            _progressLabel = new Label
            {
                Text = "0%",
                Location = new Point(1058, 12),
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

            // ====== Custom Chart Section ======
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

            // ====== Data Grid (Statistics Table) ======
            // Height reduced to make space for legend; row height increased for readability
            _resultsGrid = new DataGridView
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 145),
                BackgroundColor = Operating_Systems.Background,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            // UPDATE: Row Height increased for readability
            _resultsGrid.RowTemplate.Height = 36;

            // Styling - موحّد لخطوط محسّنة
            _resultsGrid.EnableHeadersVisualStyles = false;
            _resultsGrid.ColumnHeadersDefaultCellStyle.BackColor = Operating_Systems.PanelColor;
            _resultsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Operating_Systems.TextPrimary;
            _resultsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.25F);
            _resultsGrid.DefaultCellStyle.BackColor = Operating_Systems.Background;
            _resultsGrid.DefaultCellStyle.ForeColor = Operating_Systems.TextPrimary;
            _resultsGrid.DefaultCellStyle.SelectionBackColor = Operating_Systems.AccentBlue;
            _resultsGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // رأسياً متمركز
            _resultsGrid.GridColor = Color.FromArgb(60, 60, 60);

            // Columns
            _resultsGrid.Columns.Add("Source", "Timer Source");
            _resultsGrid.Columns.Add("Min", "Minimum (ms)");
            _resultsGrid.Columns.Add("Max", "Maximum (ms)");
            _resultsGrid.Columns.Add("Avg", "Average (ms)");
            _resultsGrid.Columns.Add("Diff", "Difference (ms)");

            currentY += _resultsGrid.Height + 6;

            // ====== Legend Section (Bottom) ======
            Panel legendPanel = new Panel
            {
                Location = new Point(10, currentY),
                Size = new Size(PanelWidth - 20, 60),
                BackColor = Color.Transparent
            };

            Font legendFont = new Font("Segoe UI", 8.25F, FontStyle.Regular);
            Color legendColor = Operating_Systems.TextSecondary;

            // شرح موجز بأعمدة: يسار ويمين لعرض EN + AR بشكل مرتب
            Label leftLegend = new Label
            {
                Text = "Minimum: Shortest measured time / أقل وقت تم رصده\nMaximum: Longest measured time / أطول وقت تم رصده",
                AutoSize = true,
                Location = new Point(0, 0),
                ForeColor = legendColor,
                Font = legendFont
            };

            Label rightLegend = new Label
            {
                Text = "Average: Arithmetic mean / المتوسط الحسابي\nDifference: Maximum - Minimum (Range) / المدى (الفرق بين العظمى والصغرى)",
                AutoSize = true,
                Location = new Point(420, 0), // Offset for second column
                ForeColor = legendColor,
                Font = legendFont
            };

            legendPanel.Controls.Add(leftLegend);
            legendPanel.Controls.Add(rightLegend);

            // ====== Add controls to holder (order matters for z-index) ======
            OS.AddToMainContainer(titleLabel);
            OS.AddToMainContainer(descLabel);
            OS.AddToMainContainer(configPanel);
            OS.AddToMainContainer(_chartPanel);
            OS.AddToMainContainer(_resultsGrid);
            OS.AddToMainContainer(legendPanel);

            // ====== Accessibility / Responsiveness notes for junior devs (Arabic) ======
            // - الحفاظ على Anchors و Dock إن رغبت بتحسين الديناميكية لاحقًا.
            // - لتعديل ارتفاع الجدول عند تغيير الخط الأساسي: عدّل RowTemplate.Height بناءً على Font.Height.
            // - إذا أردنا أن يتحجّم الوصف (descLabel) عند عرض نوافذ أصغر، فكّر في استخدام Label.AutoEllipsis = true
            // - الرسم البياني يرسم نفسه في DrawChart؛ لتعديل عنوان الرسم البياني فعّل خصائص الخط داخل دالة الرسم.
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
