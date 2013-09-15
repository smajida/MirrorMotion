using System;

namespace MirrorMotion.Core
{
    /// <summary>
    /// Adapter for Kinect Interaction lib
    /// </summary>
    public interface IKinectInteractionAdapter
    {
        /// <summary>
        /// Occurs when new InteractionHandPointer is provided
        /// </summary>
        event EventHandler<InteractionHandPointerEventArgs> HandPointerUpdated;
        /// <summary>
        /// Starts working with Kinect sensor
        /// </summary>
        void Start();
        /// <summary>
        /// Stops working with Kinect sensor
        /// </summary>
        void Stop();
    }
}