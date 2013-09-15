using System;
using Microsoft.Kinect.Toolkit.Interaction;
using MirrorMotion.Core.Properties;

namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Swipe with closed hand
    /// </summary>
    public class GrippedSwipeGesture : SingleGesture
    {
        private bool _gripStarted;
        private double _startX, _startY, _startZ;

        public GrippedSwipeGesture(IKinectInteractionAdapter interactionAdapter) : base(interactionAdapter)
        {
        }

        protected override void OnHandPointerUpdated(object sender, InteractionHandPointerEventArgs e)
        {
            if (e.HandPointer.HandEventType == InteractionHandEventType.Grip)
            {
                _gripStarted = true;
                _startX = e.HandPointer.X;
                _startY = e.HandPointer.Y;
                _startZ = e.HandPointer.RawZ;
            }
            else if (e.HandPointer.HandEventType == InteractionHandEventType.GripRelease)
            {
                _startY = e.HandPointer.Y;

                if (_gripStarted &&
                    IsSwipe(_startX, _startY, _startZ, e.HandPointer.X, e.HandPointer.Y, e.HandPointer.RawZ))
                    RaiseOccur();

                _gripStarted = false;
            }
        }

        /// <summary>
        /// Determines whether a change of user hand's position satisfies criteria of a "swipe"
        /// </summary>
        protected virtual bool IsSwipe(double startX, double startY, double startZ,
                                       double endX, double endY, double endZ)
        {
            return Math.Abs(endX - startX) > Detection.Default.GrippedSwipe_MainAxisMinDistance
                   || Math.Abs(endY - startY) > Detection.Default.GrippedSwipe_MainAxisMinDistance
                   || Math.Abs(endZ - startZ) > Detection.Default.GrippedSwipe_MainAxisMinDistance;
        }
    }
}