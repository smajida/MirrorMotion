using System;
using MirrorMotion.Core.Properties;

namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Horizontal right swipe with closed hand
    /// </summary>
    public class GrippedHorizontalRightSwipeGesture : GrippedSwipeGesture
    {
        public GrippedHorizontalRightSwipeGesture(IKinectInteractionAdapter interactionAdapter) : base(interactionAdapter)
        {
            InteractionAdapter = interactionAdapter;
        }

        protected override bool IsSwipe(double startX, double startY, double startZ, double endX, double endY, double endZ)
        {
            return endX - startX > Detection.Default.GrippedSwipe_MainAxisMinDistance
                   && Math.Abs(endY - startY) < Detection.Default.GrippedSwipe_OtherAxesMaxDistance
                   && Math.Abs(endZ - startZ) < Detection.Default.GrippedSwipe_OtherAxesMaxDistance;
        }
    }
}