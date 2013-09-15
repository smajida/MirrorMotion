using System;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Interaction;

namespace MirrorMotion.Core
{
    /// <summary>
    /// Adapter for Kinect Interaction lib
    /// Code is mostly from Kinect Developer Toolkit 1.7
    /// </summary>
    public class KinectInteractionAdapter : IKinectInteractionAdapter, IInteractionClient
    {
        private readonly KinectSensorChooser _kinectSensorChooser;

        private KinectSensor KinectSensor
        {
            get { return _kinectSensorChooser.Kinect; }
        }

        /// <summary>
        /// Entry point for interaction stream functionality.
        /// </summary>
        private InteractionStream _interactionStream;

        /// <summary>
        /// Intermediate storage for the skeleton data received from the Kinect sensor.
        /// </summary>
        private Skeleton[] _skeletons;

        /// <summary>
        /// Intermediate storage for the user information received from interaction stream.
        /// </summary>
        private UserInfo[] _userInfos;

        public event EventHandler<InteractionHandPointerEventArgs> HandPointerUpdated;

        public KinectInteractionAdapter(KinectSensorChooser kinectSensorChooser)
        {
            _kinectSensorChooser = kinectSensorChooser;
            _kinectSensorChooser.KinectChanged += OnSensorChanged;
        }

        public void Start()
        {
            _kinectSensorChooser.Start();
        }

        public void Stop()
        {
            _kinectSensorChooser.Stop();
        }

        private void OnSensorChanged(object sender, KinectChangedEventArgs kinectChangedEventArgs)
        {
            var oldSensor = kinectChangedEventArgs.OldSensor;
            var newSensor = kinectChangedEventArgs.NewSensor;

            if (oldSensor != null)
            {
                try
                {
                    oldSensor.DepthStream.Range = DepthRange.Default;
                    oldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    oldSensor.DepthStream.Disable();
                    oldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }

                oldSensor.DepthFrameReady -= this.SensorDepthFrameReady;
                oldSensor.SkeletonFrameReady -= this.SensorSkeletonFrameReady;

                this._skeletons = null;
                this._userInfos = null;

                this._interactionStream.InteractionFrameReady -= this.InteractionFrameReady;
                this._interactionStream.Dispose();
                this._interactionStream = null;
            }

            if (newSensor != null)
            {
                try
                {
                    newSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    newSensor.SkeletonStream.Enable();

                    try
                    {
                        newSensor.DepthStream.Range = DepthRange.Near;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        newSensor.DepthStream.Range = DepthRange.Default;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }

                this._interactionStream = new InteractionStream(newSensor, this);
                this._interactionStream.InteractionFrameReady += this.InteractionFrameReady;

                // Allocate space to put the skeleton and interaction data we'll receive
                this._skeletons = new Skeleton[newSensor.SkeletonStream.FrameSkeletonArrayLength];
                this._userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];

                newSensor.DepthFrameReady += this.SensorDepthFrameReady;
                newSensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
            }
        }

        /// <summary>
        /// Handler for the Kinect sensor's DepthFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="depthImageFrameReadyEventArgs">event arguments</param>
        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs depthImageFrameReadyEventArgs)
        {
            // Even though we un-register all our event handlers when the sensor
            // changes, there may still be an event for the old sensor in the queue
            // due to the way the KinectSensor delivers events.  So check again here.
            if (this.KinectSensor != sender)
            {
                return;
            }

            using (DepthImageFrame depthFrame = depthImageFrameReadyEventArgs.OpenDepthImageFrame())
            {
                if (null != depthFrame)
                {
                    try
                    {
                        // Hand data to Interaction framework to be processed
                        this._interactionStream.ProcessDepth(depthFrame.GetRawPixelData(), depthFrame.Timestamp);
                    }
                    catch (InvalidOperationException)
                    {
                        // DepthFrame functions may throw when the sensor gets
                        // into a bad state.  Ignore the frame in that case.
                    }
                }
            }
        }

        /// <summary>
        /// Handler for the Kinect sensor's SkeletonFrameReady event
        /// </summary>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
        {
            // Even though we un-register all our event handlers when the sensor
            // changes, there may still be an event for the old sensor in the queue
            // due to the way the KinectSensor delivers events.  So check again here.
            if (this.KinectSensor != sender)
            {
                return;
            }

            using (SkeletonFrame skeletonFrame = skeletonFrameReadyEventArgs.OpenSkeletonFrame())
            {
                if (null != skeletonFrame)
                {
                    try
                    {
                        // Copy the skeleton data from the frame to an array used for temporary storage
                        skeletonFrame.CopySkeletonDataTo(this._skeletons);

                        var accelerometerReading = this.KinectSensor.AccelerometerGetCurrentReading();

                        // Hand data to Interaction framework to be processed
                        this._interactionStream.ProcessSkeleton(this._skeletons, accelerometerReading,
                                                                skeletonFrame.Timestamp);
                    }
                    catch (InvalidOperationException)
                    {
                        // SkeletonFrame functions may throw when the sensor gets
                        // into a bad state.  Ignore the frame in that case.
                    }
                }
            }
        }

        InteractionInfo IInteractionClient.GetInteractionInfoAtLocation(int skeletonTrackingId,
                                                                        InteractionHandType handType,
                                                                        double x, double y)
        {
            var interactionInfo = new InteractionInfo
                {
                    IsPressTarget = true,
                    IsGripTarget = true,
                };

            return interactionInfo;
        }

        /// <summary>
        /// Event handler for InteractionStream's InteractionFrameReady event
        /// </summary>
        private void InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            // Check for a null userInfos since we may still get posted events
            // from the stream after we have unregistered our event handler and
            // deleted our buffers.
            if (this._userInfos == null)
            {
                return;
            }

            UserInfo[] localUserInfos = null;

            using (InteractionFrame interactionFrame = e.OpenInteractionFrame())
            {
                if (interactionFrame != null)
                {
                    // Copy interaction frame data so we can dispose interaction frame
                    // right away, even if data processing/event handling takes a while.
                    interactionFrame.CopyInteractionDataTo(this._userInfos);
                    localUserInfos = this._userInfos;
                }
            }

            if (localUserInfos != null)
            {
                bool wasProcessingAborted = false;

                // Raise events based on the state of all hand pointers
                for (int userIndex = 0; userIndex < localUserInfos.Length; ++userIndex)
                {
                    var user = localUserInfos[userIndex];
                    foreach (var handPointer in user.HandPointers)
                    {
                        if (HandPointerUpdated != null)
                            HandPointerUpdated(this, new InteractionHandPointerEventArgs(handPointer));

                        if (localUserInfos != this._userInfos)
                        {
                            // Double-check that user info data being processed is still valid.
                            // Client might have invalidated it by changing the KinectSensor
                            wasProcessingAborted = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}