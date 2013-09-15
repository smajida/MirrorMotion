using Microsoft.Kinect.Toolkit;
using MirrorMotion.Actions;
using MirrorMotion.Core;
using MirrorMotion.Core.Gestures;

namespace MirrorMotion.Console
{
    internal class Program
    {
        /// <summary>
        /// Console demo application that tracks user's gestures and reports about them
        /// </summary>
        private static void Main(string[] args)
        {
            System.Console.WriteLine("press [ENTER] to exit");

            var sensorChooser = new KinectSensorChooser();
            sensorChooser.KinectChanged += (sender, e) =>
                {
                    if (e.NewSensor != null)
                    {
                        System.Console.WriteLine("sensor initialized");
                    }
                };

            var interactionAdapter = new KinectInteractionAdapter(sensorChooser);

            // initialize demo gesture
            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new GrippedHorizontalRightSwipeGesture(interactionAdapter));
            complexGesture.Add(new GrippedSwipeGesture(interactionAdapter));

            // initialize demo MirrorMotion agent
            var agent = new MirrorMotionAgent
                {
                    Trigger = complexGesture,
                    Action = new CloseForegroundWindowAction().Execute
                };
            agent.Start();

            interactionAdapter.Start();

            System.Console.ReadLine();

            agent.Stop();
            interactionAdapter.Stop();
        }
    }
}