using System;
using System.Windows.Forms;
using Timer = System.Threading.Timer;
using Timeout = System.Threading.Timeout;

namespace AppEmailFernando.Components
{
    /// <summary>
    /// Implementation of an auto closing message box with timeout
    /// </summary>
    public class AutoClosingMessageBox
    {
        #region ImportDLL

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        #endregion

        internal Timer _timeoutTimer;
        
        internal string _caption;

        private AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new Timer(OnTimerElapsed, null, timeout, Timeout.Infinite);

            MessageBox.Show(text, caption);
        }

        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        private void OnTimerElapsed(object state)
        {
            IntPtr window = FindWindow(null, _caption);

            if (window != IntPtr.Zero)
                SendMessage(window, 0x0010, IntPtr.Zero, IntPtr.Zero);

            _timeoutTimer.Dispose();
        }
    }
}
