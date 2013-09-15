using System;
using MirrorMotion.Core.Gestures;

namespace MirrorMotion.Core
{
    /// <summary>
    /// Agent that runs an action when user makes a gesture
    /// </summary>
    public class MirrorMotionAgent
    {
        /// <summary>
        /// Gesture that triggers action
        /// </summary>
        public Gesture Trigger { get; set; }
        
        /// <summary>
        /// Action which is triggered by user's gesture
        /// </summary>
        public Action Action { get; set; }

        public MirrorMotionAgent()
        {
        }

        public MirrorMotionAgent(Gesture gesture, Action action)
        {
            Trigger = gesture;
            Action = action;
        }

        /// <summary>
        /// Starts tracking gestures
        /// </summary>
        public void Start()
        {
            if (Trigger == null || Action == null)
                throw new NotInitializedException();

            Trigger.Occur += DoAction;
            Trigger.StartTracking();
        }

        /// <summary>
        /// Stops tracking gestures
        /// </summary>
        public void Stop()
        {
            Trigger.StopTracking();
            Trigger.Occur -= DoAction;
        }

        private void DoAction(object sender, EventArgs e)
        {
            Action.Invoke();
        }
    }
}