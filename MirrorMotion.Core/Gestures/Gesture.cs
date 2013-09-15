using System;

namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Base class for user's gesture
    /// </summary>
    public abstract class Gesture
    {
        /// <summary>
        /// Signals that user made a gesture
        /// </summary>
        public event EventHandler Occur;

        protected virtual void RaiseOccur()
        {
            EventHandler handler = Occur;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Starts tracking user's gestures
        /// </summary>
        public abstract void StartTracking();

        /// <summary>
        /// Stops tracking user's gestures
        /// </summary>
        public abstract void StopTracking();
    }
}