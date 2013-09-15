using System;
using Microsoft.Kinect.Toolkit.Interaction;

namespace MirrorMotion.Core
{
    /// <summary>
    /// Event data that contains InteractionHandPointer
    /// </summary>
    public class InteractionHandPointerEventArgs : EventArgs
    {
        /// <summary>
        /// InteractionHandPointer associated with event
        /// </summary>
        public InteractionHandPointer HandPointer { get; set; }

        public InteractionHandPointerEventArgs(InteractionHandPointer interactionHandPointer)
        {
            HandPointer = interactionHandPointer;
        }
    }
}