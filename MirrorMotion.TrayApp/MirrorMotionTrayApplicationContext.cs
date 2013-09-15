using System;
using System.Windows.Forms;
using MirrorMotion.TrayApp.Properties;

namespace MirrorMotion.TrayApp
{
    public class MirrorMotionTrayApplicationContext : ApplicationContext
    {
        public NotifyIcon NotifyIcon;

        public MirrorMotionTrayApplicationContext()
        {
            NotifyIcon = new NotifyIcon
                {
                    ContextMenuStrip = BuildContextMenu(),
                    Icon = Resources.trayicon,
                    Text = Settings.Default.DefaultTooltip,
                    Visible = true
                };
        }

        private static ContextMenuStrip BuildContextMenu()
        {
            var menuStrip = new ContextMenuStrip();

            menuStrip.Items.Add("Stop Tracking && Quit", null, OnExit_Click);

            return menuStrip;
        }

        private static void OnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}