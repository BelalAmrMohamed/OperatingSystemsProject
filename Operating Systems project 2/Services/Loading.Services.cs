using System;
using System.Linq;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class Loading
    {
        private static void Parent_Resize(object sender, EventArgs e)
        {
            if (_card == null || !(sender is Form parent)) return;

            _card.Left = (parent.ClientSize.Width - _card.Width) / 2;
            _card.Top = (parent.ClientSize.Height - _card.Height) / 2;
        }

        /// <summary>
        /// Stop and remove the loading overlay if present.
        /// </summary>
        public static void StopLoading()
        {
            if (_overlay == null) return;

            // Find parent by walking up from card if possible
            Form parent = _card?.FindForm() ?? Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.GetType().Name == "Operating_Systems");
            if (parent == null)
            {
                // try to clean up without parent
                _marquee?.Dispose();
                _messageLabel?.Dispose();
                _titleLabel?.Dispose();
                _card?.Dispose();
                _overlay?.Dispose();

                _marquee = null;
                _messageLabel = null;
                _titleLabel = null;
                _card = null;
                _overlay = null;
                return;
            }

            void destroy()
            {
                parent.Resize -= Parent_Resize;

                if (_card != null && parent.Controls.Contains(_card)) parent.Controls.Remove(_card);
                if (_overlay != null && parent.Controls.Contains(_overlay)) parent.Controls.Remove(_overlay);

                _marquee?.Dispose();
                _messageLabel?.Dispose();
                _titleLabel?.Dispose();
                _card?.Dispose();
                _overlay?.Dispose();

                _marquee = null;
                _messageLabel = null;
                _titleLabel = null;
                _card = null;
                _overlay = null;
            }

            if (parent.InvokeRequired)
                parent.Invoke((Action)destroy);
            else
                destroy();
        }

        /// <summary>
        /// Update the message text if overlay already exists.
        /// </summary>
        private static void UpdateMessage(string message)
        {
            if (_messageLabel == null) return;

            var parent = _messageLabel.FindForm();
            if (parent != null && parent.InvokeRequired)
            {
                parent.Invoke((Action)(() => _messageLabel.Text = message));
            }
            else
            {
                _messageLabel.Text = message;
            }
        }
    }
}