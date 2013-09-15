namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Base class for single atomic gesture
    /// </summary>
    public abstract class SingleGesture : Gesture
    {
        protected IKinectInteractionAdapter InteractionAdapter { get; set; }

        protected SingleGesture(IKinectInteractionAdapter interactionAdapter)
        {
            InteractionAdapter = interactionAdapter;
        }

        public override void StartTracking()
        {
            InteractionAdapter.HandPointerUpdated += OnHandPointerUpdated;
        }

        public override void StopTracking()
        {
            InteractionAdapter.HandPointerUpdated -= OnHandPointerUpdated;
        }

        protected abstract void OnHandPointerUpdated(object sender, InteractionHandPointerEventArgs e);
    }
}