using System;
using System.Windows.Forms;
using Microsoft.Kinect.Toolkit;
using MirrorMotion.Actions;
using MirrorMotion.Core;
using MirrorMotion.Core.Gestures;
using MirrorMotion.TrayApp.Properties;

namespace MirrorMotion.TrayApp
{
    /// <summary>
    /// Demo of an application that stays in a tray and tracks user's gestures
    /// </summary>
    internal static class Program
    {
        private static MirrorMotionTrayApplicationContext Context { get; set; }

        private static IKinectInteractionAdapter KinectInteractionAdapter { get; set; }
        private static MirrorMotionAgent Agent { get; set; }

        [STAThread]
        private static void Main()
        {
            Application.ApplicationExit += Application_ApplicationExit;

            Context = new MirrorMotionTrayApplicationContext();

            InitializeMirrorMotion();

            Application.Run(Context);
        }

        private static void InitializeMirrorMotion()
        {
            var sensorChooser = new KinectSensorChooser();
            sensorChooser.KinectChanged += (sender, e) =>
            {
                if (e.NewSensor != null)
                {
                    Context.NotifyIcon.Text = Settings.Default.SensorEnabledTooltip;
                }
            };

            KinectInteractionAdapter = new KinectInteractionAdapter(sensorChooser);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new GrippedHorizontalRightSwipeGesture(KinectInteractionAdapter));

            Agent = new MirrorMotionAgent
            {
                Trigger = complexGesture,
                Action = new CloseForegroundWindowAction().Execute
            };
            Agent.Start();
            KinectInteractionAdapter.Start();
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Agent.Stop();
            KinectInteractionAdapter.Stop();
        }
    }
}