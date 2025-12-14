using System;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class PowerOptions
    {
        public static void ShowPowerOption(Operating_Systems OperatingSystems)
        {
            const int PanelWidth = 1110;
            int currentY = 62;

            Label HeaderLabel = new Label
            {
                Text = "🔋 Power Options",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                MinimumSize = new Size(200, 30),
                Location = new Point(457, 8),
                Size = new Size(200, 30),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.HeaderColor,
            };
            OperatingSystems.AddToMainContainer(HeaderLabel);

            Label SubHeaderLabel = new Label
            {
                Text = "Manage system power states.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                MinimumSize = new Size(300, 20),
                Location = new Point(414, 38),
                Size = new Size(300, 20),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.TextPrimary,
            };
            OperatingSystems.AddToMainContainer(SubHeaderLabel);

            Label WarningLabel = new Label
            {
                Text = "⚠ Warning: These actions will affect your computer immediately. Make sure to save your work before proceeding.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                MinimumSize = new Size(300, 20),
                Location = new Point(240, 60),
                Size = new Size(300, 20),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.IndianRed,
            };
            OperatingSystems.AddToMainContainer(WarningLabel);

            // --- Button container ---
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY + 40),
                Size = new Size(PanelWidth, 100),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false
            };

            Size buttonSize = new Size(265, 70);

            Button shutdownBtn = CreateStyledButton("Shutdown", buttonSize, Operating_Systems.ErrorColor, ShutDown);
            Button restartBtn = CreateStyledButton("Restart", buttonSize, Operating_Systems.ErrorColor, Restart);
            Button hibernateBtn = CreateStyledButton("Hibernate", buttonSize, Operating_Systems.AccentBlue, Hibernate);
            Button sleepBtn = CreateStyledButton("Sleep", buttonSize, Color.FromArgb(108, 117, 125), Sleep);

            buttonFlow.Controls.Add(shutdownBtn);
            buttonFlow.Controls.Add(restartBtn);
            buttonFlow.Controls.Add(hibernateBtn);
            buttonFlow.Controls.Add(sleepBtn);

            OperatingSystems.AddToMainContainer(buttonFlow);
        }

        private static Button CreateStyledButton(string text, Size size, Color backgroundColor, Action action)
        {
            Color hoverColor = ControlPaint.Dark(backgroundColor, 0.15f);

            Button button = new Button
            {
                Text = text,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                BackColor = backgroundColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };

            button.FlatAppearance.BorderSize = 0;

            button.Click += (s, e) => { Confirm.Run(text, action); };

            // --- Hover effect ---
            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = backgroundColor;

            return button;
        }
    }
}