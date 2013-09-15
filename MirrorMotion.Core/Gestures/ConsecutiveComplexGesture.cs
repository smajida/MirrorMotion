using System;
using System.Collections.Generic;
using MirrorMotion.Core.Properties;

namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Sequence of gestures
    /// </summary>
    public class ConsecutiveComplexGesture : ComplexGesture
    {
        private int _currentGestureIndex;
        private DateTime _lastGestureOccurenceTime;

        /// <summary>
        /// Sequence of gestures
        /// </summary>
        private readonly List<SingleGesture> _gestures = new List<SingleGesture>();

        /// <summary>
        /// Adds a single gesture that should be a part of a sequence
        /// </summary>
        public override void Add(SingleGesture singleGesture)
        {
            _gestures.Add(singleGesture);

            if (_gestures.Count == 1)
            {
                singleGesture.Occur += OneMoreGestureOccured;
            }
        }

        public override void StartTracking()
        {
            if (_gestures.Count > 0)
            {
                _gestures[0].StartTracking();
            }
        }

        public override void StopTracking()
        {
            if (_gestures.Count > 0)
            {
                if (_currentGestureIndex > 0)
                {
                    _gestures[_currentGestureIndex].Occur -= OneMoreGestureOccured;
                    _gestures[_currentGestureIndex].StopTracking();
                    _gestures[0].Occur += OneMoreGestureOccured;
                    _currentGestureIndex = 0;
                }
                _gestures[0].StopTracking();
            }
        }

        /// <summary>
        /// Handles occurence of the last gesture in a sequence
        /// </summary>
        private void OneMoreGestureOccured(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            if (_gestures.Count == 1)
            {
                RaiseOccur();
                return;
            }

            // we don't want to track current gesture anymore
            _gestures[_currentGestureIndex].Occur -= OneMoreGestureOccured;
            _gestures[_currentGestureIndex].StopTracking();

            // time elapsed since occurence of a previous gesture in a sequence
            var elapsed = (now - _lastGestureOccurenceTime).TotalMilliseconds;

            // if a first gesture in a sequence occured or little time passed since a previous gesture's occurence
            if (_currentGestureIndex == 0 ||
                (elapsed > Detection.Default.ConsecutiveComplexGesture_MinPeriodBetweenGestures &&
                 elapsed < Detection.Default.ConsecutiveComplexGesture_MaxPeriodBetweenGestures))
            {
                // if a last gesture in a sequence occured
                if (_currentGestureIndex == _gestures.Count - 1)
                {
                    RaiseOccur();
                    StartAgain();
                }
                else
                {
                    // start tracking of a next gesture in a sequence
                    _gestures[_currentGestureIndex + 1].Occur += OneMoreGestureOccured;
                    _gestures[_currentGestureIndex + 1].StartTracking();
                    _currentGestureIndex++;
                }
            }
            else
            {
                // return to tracking of a first gesture in a sequence
                StartAgain();
            }
            _lastGestureOccurenceTime = DateTime.Now;
        }

        /// <summary>
        /// Restarts tracking of a first gesture in a sequence
        /// </summary>
        private void StartAgain()
        {
            if (_currentGestureIndex > 0)
            {
                _gestures[0].Occur += OneMoreGestureOccured;
                _gestures[0].StartTracking();
            }
            _currentGestureIndex = 0;
        }
    }
}