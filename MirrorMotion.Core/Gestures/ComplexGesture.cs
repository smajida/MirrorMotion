namespace MirrorMotion.Core.Gestures
{
    /// <summary>
    /// Base class for gesture that consist of other gestures
    /// </summary>
    public abstract class ComplexGesture : Gesture
    {
        /// <summary>
        /// Adds a single gesture that should be a part of a complex gesture
        /// </summary>
        public abstract void Add(SingleGesture singleGesture);
    }
}