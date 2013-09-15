using System;
using System.Threading;

namespace MirrorMotion.Core.Tests
{
    public class TestKinectInteractionAdapter : IKinectInteractionAdapter
    {
        private readonly int _raiseCount;
        private readonly int _periodBetweenRaises;

        public event EventHandler<InteractionHandPointerEventArgs> HandPointerUpdated;

        protected virtual void RaiseHandPointerUpdated()
        {
            var handler = HandPointerUpdated;
            if (handler != null) handler(this, null);
        }

        /// <param name="raiseCount">How many times HandPointerUpdated event will be raised</param>
        /// <param name="periodBetweenRaises">Period in msec between raises of HandPointerUpdated event</param>
        public TestKinectInteractionAdapter(int raiseCount, int periodBetweenRaises)
        {
            _raiseCount = raiseCount;
            _periodBetweenRaises = periodBetweenRaises;
        }

        public void Start()
        {
            for (int i = 0; i < _raiseCount; i++)
            {
                RaiseHandPointerUpdated();
                Thread.Sleep(_periodBetweenRaises);
            }
        }

        public void Stop()
        {
        }
    }
}