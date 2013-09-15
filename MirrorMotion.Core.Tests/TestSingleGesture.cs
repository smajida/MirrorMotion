using MirrorMotion.Core.Gestures;

namespace MirrorMotion.Core.Tests
{
    public class TestSingleGesture : SingleGesture
    {
        public TestSingleGesture(IKinectInteractionAdapter interactionAdapter) : base(interactionAdapter)
        {
        }

        protected override void OnHandPointerUpdated(object sender, InteractionHandPointerEventArgs e)
        {
            RaiseOccur();
        }
    }
}