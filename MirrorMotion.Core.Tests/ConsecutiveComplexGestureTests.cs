using Microsoft.VisualStudio.TestTools.UnitTesting;
using MirrorMotion.Core.Gestures;

namespace MirrorMotion.Core.Tests
{
    [TestClass]
    public class ConsecutiveComplexGestureTests
    {
        private const int CorrectPeriodBetweenEvents = 100;
        private const int IncorrectPeriodBetweenEvents = 1100;

        [TestMethod]
        public void OneGesture_SingleOccurence()
        {
            // arrange
            const int occurenceCount = 1;
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount, CorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) => occurenceCounter++;

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount, occurenceCounter);
        }

        [TestMethod]
        public void OneGesture_MultipleOccurences()
        {
            // arrange
            const int occurenceCount = 3;
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount, CorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) => occurenceCounter++;

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount, occurenceCounter);
        }

        [TestMethod]
        public void TwoGestures_SingleOccurence()
        {
            // arrange
            const int occurenceCount = 1;
            // two times more single gesture events should be raised
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount * 2, CorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) => occurenceCounter++;

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount, occurenceCounter);
        }

        [TestMethod]
        public void TwoGestures_MultipleOccurences()
        {
            // arrange
            const int occurenceCount = 3;
            // two times more single gesture events should be raised
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount * 2, CorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) => occurenceCounter++;

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount, occurenceCounter);
        }

        [TestMethod]
        public void TwoGestures_InterruptedSequence()
        {
            // arrange
            const int occurenceCount = 1;
            // two times more single gesture events should be raised
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount * 2, IncorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) => occurenceCounter++;

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(0, occurenceCounter);
        }

        [TestMethod]
        public void Start_Stop_Start()
        {
            // arrange
            const int occurenceCount = 3;
            // two times more single gesture events should be raised
            var kinectInteractionAdapter = new TestKinectInteractionAdapter(occurenceCount * 2, CorrectPeriodBetweenEvents);

            var complexGesture = new ConsecutiveComplexGesture();
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));
            complexGesture.Add(new TestSingleGesture(kinectInteractionAdapter));

            bool isTrackingStopped = false;
            int occurenceCounter = 0;
            complexGesture.Occur += (sender, e) =>
                {
                    occurenceCounter++;
                    // stop tracking after first gesture
                    if (occurenceCounter == 1)
                    {
                        complexGesture.StopTracking();
                        isTrackingStopped = true;
                    }
                };
            // start tracking again if it's stopped
            kinectInteractionAdapter.HandPointerUpdated += (sender, e) =>
            {
                if (isTrackingStopped)
                    complexGesture.StartTracking();
            };

            complexGesture.StartTracking();

            // act
            kinectInteractionAdapter.Start();

            // assert
            Assert.AreEqual(occurenceCount - 1, occurenceCounter);
        }
    }
}
