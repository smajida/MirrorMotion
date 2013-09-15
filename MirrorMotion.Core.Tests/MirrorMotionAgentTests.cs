using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MirrorMotion.Core.Tests
{
    [TestClass]
    public class MirrorMotionAgentTests
    {
        private const int CorrectPeriodBetweenEvents = 100;

        [TestMethod]
        public void AgentTriggersActions()
        {
            // arrange
            const int occurenceCount = 4;
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount, CorrectPeriodBetweenEvents);

            int occurenceCounter = 0;
            var agent = new MirrorMotionAgent
                {
                    Trigger = new TestSingleGesture(kinectInteractionAdapter),
                    Action = () => occurenceCounter++
                };

            agent.Start();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount, occurenceCounter);
        }
    }
}
