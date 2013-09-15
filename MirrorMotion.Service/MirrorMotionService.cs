using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Kinect.Toolkit;
using MirrorMotion.Core;
using MirrorMotion.Core.Gestures;

namespace MirrorMotion.Service
{
    /// <summary>
    /// Demo of Windows Service that tracks user's gestures and writes in event log about them
    /// </summary>
    public partial class MirrorMotionService : ServiceBase
    {
        private const string Source = "MirrorMotionService";

        private IKinectInteractionAdapter KinectInteractionAdapter { get; set; }
        private MirrorMotionAgent Agent { get; set; }

        public MirrorMotionService()
        {
            InitializeComponent();

            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, "");
            }
            eventLog.Source = Source;
        }

        protected override void OnStart(string[] args)
        {
            var sensorChooser = new KinectSensorChooser();
            sensorChooser.KinectChanged += (sender, e) =>
            {
                if (e.NewSensor != null)
                {
                    eventLog.WriteEntry("Kinect sensor initialized");
                }
            };

            KinectInteractionAdapter = new KinectInteractionAdapter(sensorChooser);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new GrippedHorizontalRightSwipeGesture(KinectInteractionAdapter));

            Agent = new MirrorMotionAgent
            {
                Trigger = complexGesture,
                Action = () => eventLog.WriteEntry("MirrorMotion action triggered")
            };
            Agent.Start();
            KinectInteractionAdapter.Start();
        }

        protected override void OnStop()
        {
            Agent.Stop();
            KinectInteractionAdapter.Stop();
        }
    }
}