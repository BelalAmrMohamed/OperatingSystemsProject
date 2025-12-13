using System;
using System.Linq;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class Loading
    {
        // Keep card centered when parent resizes
        private static void Parent_Resize(object sender, EventArgs e)
        {
            if (_card == null || !(sender is Form parent)) return;

            _card.Left = (parent.ClientSize.Width - _card.Width) / 2;
            _card.Top = (parent.ClientSize.Height - _card.Height) / 2;
        }

        /// <summary>
        /// Stop and remove the loading overlay if present.
        /// Properly disposes the spinner and unsubscribes events.
        /// </summary>
        public static void StopLoading()
        {
            // nothing to do
            if (_overlay == null && _card == null) return;

            // try to find parent
            Form parent = _card?.FindForm() ?? Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.GetType().Name == "Operating_Systems");
            if (parent == null)
            {
                // fallback: dispose everything safely
                DisposeAll();
                return;
            }

            void destroy()
            {
                try
                {
                    parent.Resize -= Parent_Resize;
                }
                catch { /* ignore if not subscribed */ }

                if (_card != null && parent.Controls.Contains(_card)) parent.Controls.Remove(_card);
                if (_overlay != null && parent.Controls.Contains(_overlay)) parent.Controls.Remove(_overlay);

                DisposeAll();
            }

            if (parent.InvokeRequired)
                parent.Invoke((Action)destroy);
            else
                destroy();
        }

        // centralized cleanup used by StopLoading
        private static void DisposeAll()
        {
            try
            {
                // Stop and dispose spinner
                try
                {
                    _spinner?.Stop();
                    _spinner?.Dispose();
                }
                catch { /* ignore */ }
                _spinner = null;

                // Dispose labels and panels
                try { _messageLabel?.Dispose(); } catch { }
                _messageLabel = null;

                try { _titleLabel?.Dispose(); } catch { }
                _titleLabel = null;

                try { _card?.Dispose(); } catch { }
                _card = null;

                try { _overlay?.Dispose(); } catch { }
                _overlay = null;
            }
            catch
            {
                // swallow; we do our best to clean up
            }
        }
    }
}