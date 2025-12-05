using System;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    public partial class AreYouSure : Form
    {
        private static string _header;
        private static Action _activateFunction;

        /// <summary>
        /// Displays a confirmation dialog with the specified message and action
        /// </summary>
        /// <param name="text">The action description (e.g., "delete this file")</param>
        /// <param name="function">The function to execute if user confirms</param>
        public static void Run(string text, Action function)
        {
            _header = text ?? string.Empty;
            _activateFunction = function;

            using (AreYouSure dialog = new AreYouSure())
            {
                dialog.ShowDialog();
            }
        }

        public AreYouSure()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Form properties
            this.Text = "Confirmation";
            this.ClientSize = new Size(450, 160);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Icon (warning icon)
            PictureBox iconBox = new PictureBox
            {
                Size = new Size(48, 48),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = SystemIcons.Warning.ToBitmap()
            };

            // Message label
            Label messageLabel = new Label
            {
                Text = $"Are you sure you want to {_header}?",
                Location = new Point(80, 20),
                Size = new Size(350, 60),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Button panel for consistent spacing
            Panel buttonPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(0, 10, 15, 10)
            };

            // Yes button
            Button yesButton = new Button
            {
                Text = "Yes",
                Size = new Size(100, 32),
                Location = new Point(buttonPanel.Width - 220, 9),
                Anchor = AnchorStyles.Right,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.System,
                DialogResult = DialogResult.Yes
            };
            yesButton.Click += YesButton_Click;

            // Cancel button
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 32),
                Location = new Point(buttonPanel.Width - 110, 9),
                Anchor = AnchorStyles.Right,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.System,
                DialogResult = DialogResult.Cancel
            };
            cancelButton.Click += CancelButton_Click;

            // Add controls
            buttonPanel.Controls.Add(yesButton);
            buttonPanel.Controls.Add(cancelButton);

            this.Controls.Add(iconBox);
            this.Controls.Add(messageLabel);
            this.Controls.Add(buttonPanel);

            // Set default buttons
            this.AcceptButton = yesButton;
            this.CancelButton = cancelButton;
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            try
            {
                _activateFunction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}