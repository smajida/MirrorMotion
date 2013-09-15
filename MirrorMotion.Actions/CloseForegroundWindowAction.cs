using System;
using System.Runtime.InteropServices;

namespace MirrorMotion.Actions
{
    /// <summary>
    /// Class encapsulates closing of current foreground window
    /// </summary>
    public class CloseForegroundWindowAction
    {
        #region Functions imported from user32.dll

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr EndTask(IntPtr hWnd, bool fShutDown, bool fForce);

        #endregion

        /// <summary>
        /// Closing of foreground window
        /// </summary>
        public void Execute()
        {
            var fgw = GetForegroundWindow();

            if (fgw == IntPtr.Zero || fgw == GetDesktopWindow() || fgw == GetShellWindow())
                return;

            EndTask(fgw, false, false);

            // Bring other window (top one now) to foreground
            var tw = GetTopWindow(IntPtr.Zero);
            if (tw != IntPtr.Zero)
                SetForegroundWindow(tw);
        }
    }
}