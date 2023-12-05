// <copyright file="GeospatialController.cs" company="Google LLC">
//
// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
// PLATEAU SDK AR Extensions for Unity Samples
//
// Modified by: Shohei Miyashita (Unity Technologies Japan)
// Date: 2023/09/22
// Changes:
// - Applied the naming rules and coding standards of PLATEAU AR Toolkit.
// - Removed the extra features like the UI controller.
// - Added an interface for checking availability of AR.
// - Improved error handling.
// - Added more comments for entities.

using Google.XR.ARCoreExtensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using PlateauToolkit.AR;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace PlateauAR.Geospatial
{
    /// <summary>
    /// Controller for Geospatial API
    /// </summary>
    /// <remarks>
    /// This class initiates the main process of Geospatial API like permission authorization,
    /// starting some services required for AR and localization.
    /// </remarks>
    public class GeospatialController : PlateauARGeospatialController
    {
        /// <summary>
        /// Error codes of Geospatial API
        /// </summary>
        public enum ErrorCode
        {
            /// <summary>
            /// Some permission required for AR was denied.
            /// </summary>
            PermissionDenied,

            /// <summary>
            /// Location service is disabled by user.
            /// </summary>
            LocationServiceDisabled,

            /// <summary>
            /// Location service ended with some status.
            /// </summary>
            LocationServiceNotRunning,

            /// <summary>
            /// ARSession error state.
            /// </summary>
            ARSessionErrorState,

            /// <summary>
            /// Geospatial sample failed to start location service.
            /// Please restart the app and grant the fine location permission.
            /// </summary>
            LocationServiceFailed,

            /// <summary>
            /// Geospatial sample failed due to missing AR Components
            /// </summary>
            MissingARComponents,

            /// <summary>
            /// The Geospatial API is not supported by this device.
            /// </summary>
            ARNotSupported,

            /// <summary>
            /// Localization not possible.
            /// </summary>
            LocalizationTimeout,
        }

        /// <summary>
        /// The ARSessionOrigin used in the sample.
        /// </summary>
        [Header("AR Components")]
        [FormerlySerializedAs("SessionOrigin")]
        public ARSessionOrigin m_SessionOrigin;

        /// <summary>
        /// The ARSession used in the sample.
        /// </summary>
        [FormerlySerializedAs("Session")]
        public ARSession m_Session;

        /// <summary>
        /// The AREarthManager used in the sample.
        /// </summary>
        [FormerlySerializedAs("EarthManager")]
        public AREarthManager m_EarthManager;

        public ARAnchorManager m_AnchorManager;

        /// <summary>
        /// The ARCoreExtensions used in the sample.
        /// </summary>
        [FormerlySerializedAs("ARCoreExtensions")]
        public ARCoreExtensions m_ARCoreExtensions;

        /// <summary>
        /// The timeout period waiting for localization to be completed.
        /// </summary>
        const float k_TimeoutSeconds = 180;

        /// <summary>
        /// Accuracy threshold for orientation yaw accuracy in degrees that can be treated as
        /// localization completed.
        /// </summary>
        const double k_OrientationYawAccuracyThreshold = 25;

        /// <summary>
        /// Accuracy threshold for altitude and longitude that can be treated as localization
        /// completed.
        /// </summary>
        const double k_HorizontalAccuracyThreshold = 20;

        /// <summary>
        /// Determines if streetscape geometry should be removed from the scene.
        /// </summary>
        bool m_ClearStreetscapeGeometryRenderObjects;

        bool m_HasError;
        bool m_HasInitialized;
        bool m_IsLocalizing;
        bool m_EnablingGeospatial;
        float m_LocalizationPassedTime;
        float m_ConfigurePrepareTime = 3f;

        Coroutine m_InitializeCoroutine;

        public event Action<ErrorCode> OnError;

        public override bool IsReady()
        {
            return m_EarthManager.EarthTrackingState == TrackingState.Tracking;
        }

        public override bool HasError()
        {
            return m_HasError;
        }

        public override bool TryGetPose(out GeospatialPose pose)
        {
            if (m_EarthManager.EarthState == EarthState.Enabled &&
                m_EarthManager.EarthTrackingState == TrackingState.Tracking)
            {
                pose = m_EarthManager.CameraGeospatialPose;
                return true;
            }

            pose = new GeospatialPose();
            return false;
        }

        public override void CreateAnchoredObject(double latitude, double longitude, double altitude, Transform obj)
        {
            ARGeospatialAnchor anchor = m_AnchorManager.AddAnchor(latitude, longitude, altitude, Quaternion.identity);
            obj.parent = anchor.transform;
        }

        IEnumerator InitializeGeospatialApi()
        {
#if UNITY_ANDROID
            yield return EnsureRequiredPermissions();
#endif
#if UNITY_IOS
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                HandleError(ErrorCode.PermissionDenied);
                yield break;
            }
#elif UNITY_ANDROID
            if (!CheckPermission())
            {
                HandleError(ErrorCode.PermissionDenied);
                yield break;
            }
#endif
            yield return StartLocationService();
            if (m_HasError)
            {
                yield break;
            }

            yield return CheckARAvailability();
            m_HasInitialized = true;
        }

        /// <summary>
        /// Unity's Awake() method.
        /// </summary>
        void Awake()
        {
            // Lock screen to portrait.
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.Portrait;

            // Enable geospatial sample to target 60fps camera capture frame rate
            // on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;

            if (m_SessionOrigin == null)
            {
                Debug.LogError("Cannot find ARSessionOrigin.");
            }

            if (m_Session == null)
            {
                Debug.LogError("Cannot find ARSession.");
            }

            if (m_ARCoreExtensions == null)
            {
                Debug.LogError("Cannot find ARCoreExtensions.");
            }
        }

        /// <summary>
        /// Unity's OnEnable() method.
        /// </summary>
        void OnEnable()
        {
            m_InitializeCoroutine = StartCoroutine(InitializeGeospatialApi());
            m_EnablingGeospatial = false;
            m_LocalizationPassedTime = 0f;
            m_IsLocalizing = true;
        }

        /// <summary>
        /// Unity's OnDisable() method.
        /// </summary>
        void OnDisable()
        {
            Input.location.Stop();

            if (m_InitializeCoroutine != null)
            {
                StopCoroutine(m_InitializeCoroutine);
                m_InitializeCoroutine = null;
                m_HasInitialized = false;
            }
        }

        /// <summary>
        /// Unity's Update() method.
        /// </summary>
        void Update()
        {
            if (m_HasError || !m_HasInitialized)
            {
                return;
            }

            // Check session error status.
            LifecycleUpdate();

            if (ARSession.state is not ARSessionState.SessionInitializing and
                not ARSessionState.SessionTracking)
            {
                return;
            }

            // Check feature support and enable Geospatial API when it's supported.
            FeatureSupported featureSupport = m_EarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            switch (featureSupport)
            {
                case FeatureSupported.Unknown:
                    return;
                case FeatureSupported.Unsupported:
                    HandleError(ErrorCode.ARNotSupported);
                    return;
                case FeatureSupported.Supported:
                    if (m_ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode == GeospatialMode.Disabled)
                    {
                        Debug.Log("Geospatial sample switched to GeospatialMode.Enabled.");
                        m_ARCoreExtensions.ARCoreExtensionsConfig.GeospatialMode = GeospatialMode.Enabled;
                        m_ARCoreExtensions.ARCoreExtensionsConfig.StreetscapeGeometryMode = StreetscapeGeometryMode.Enabled;
                        m_ConfigurePrepareTime = 3;
                        m_EnablingGeospatial = true;
                        return;
                    }

                    break;
            }

            // Waiting for new configuration to take effect.
            if (m_EnablingGeospatial)
            {
                m_ConfigurePrepareTime -= Time.deltaTime;
                if (m_ConfigurePrepareTime < 0)
                {
                    m_EnablingGeospatial = false;
                }
                else
                {
                    return;
                }
            }

            // Check earth localization.
            bool isSessionReady = ARSession.state == ARSessionState.SessionTracking &&
                                  Input.location.status == LocationServiceStatus.Running;
            TrackingState earthTrackingState = m_EarthManager.EarthTrackingState;
            GeospatialPose pose = earthTrackingState == TrackingState.Tracking
                ? m_EarthManager.CameraGeospatialPose
                : new GeospatialPose();
            if (!isSessionReady || earthTrackingState != TrackingState.Tracking ||
                pose.OrientationYawAccuracy > k_OrientationYawAccuracyThreshold ||
                pose.HorizontalAccuracy > k_HorizontalAccuracyThreshold)
            {
                // Lost localization during the session.
                if (!m_IsLocalizing)
                {
                    m_IsLocalizing = true;
                    m_LocalizationPassedTime = 0f;
                }

                if (m_LocalizationPassedTime > k_TimeoutSeconds)
                {
                    Debug.LogError("Geospatial sample localization timed out.");
                    HandleError(ErrorCode.LocalizationTimeout);
                }
                else
                {
                    m_LocalizationPassedTime += Time.deltaTime;
                }
            }
            else if (m_IsLocalizing)
            {
                // Finished localization.
                m_IsLocalizing = false;
                m_LocalizationPassedTime = 0f;
            }
            else
            {
                if (m_ClearStreetscapeGeometryRenderObjects)
                {
                    m_ClearStreetscapeGeometryRenderObjects = false;
                }
            }
        }

        #if UNITY_ANDROID
                static readonly string[] k_RequiredPermissions =
                {
                    Permission.FineLocation,
                    Permission.Camera,
                };

                static IEnumerator EnsurePermission(string permission, Action<bool> onFinish)
                {
                    if (Permission.HasUserAuthorizedPermission(permission))
                    {
                        yield break;
                    }

                    bool isDenied = false;
                    bool isFinished = false;

                    void OnDenied(string permissionName)
                    {
                        isFinished = true;
                        isDenied = true;
                    }
                    void OnGranted(string permissionName)
                    {
                        isFinished = true;
                    }

                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionDenied += OnDenied;
                    callbacks.PermissionDeniedAndDontAskAgain += OnDenied;
                    callbacks.PermissionGranted += OnGranted;

                    Permission.RequestUserPermission(permission, callbacks);

                    while (!isFinished)
                    {
                        yield return null;
                    }

                    onFinish.Invoke(!isDenied);
                }

                IEnumerator EnsureRequiredPermissions()
                {
                    foreach (string permissionName in k_RequiredPermissions)
                    {
                        bool isDenied = false;
                        yield return EnsurePermission(permissionName, isGranted =>
                        {
                            if (!isGranted)
                            {
                                isDenied = true;
                            }
                        });

                        if (isDenied)
                        {
                            HandleError(ErrorCode.PermissionDenied);
                            yield break;
                        }
                    }
                }

                bool CheckPermission()
                {
                    foreach (string permissionName in k_RequiredPermissions)
                    {
                        if (!Permission.HasUserAuthorizedPermission(permissionName))
                        {
                            return false;
                        }
                    }

                    return true;
                }
        #endif

                IEnumerator CheckARAvailability()
                {
                    Debug.Assert(Input.location.status == LocationServiceStatus.Running);

                    if (ARSession.state == ARSessionState.None)
                    {
                        yield return ARSession.CheckAvailability();
                    }

                    // Waiting for ARSessionState.CheckingAvailability.
                    yield return null;

                    if (ARSession.state == ARSessionState.NeedsInstall)
                    {
                        yield return ARSession.Install();
                    }

                    // Waiting for ARSessionState.Installing.
                    yield return null;

                    // Update event is executed before coroutines so it checks the latest error states.
                    if (m_HasError)
                    {
                        yield break;
                    }

                    LocationInfo location = Input.location.lastData;
                    VpsAvailabilityPromise vpsAvailabilityPromise =
                        AREarthManager.CheckVpsAvailabilityAsync(location.latitude, location.longitude);
                    yield return vpsAvailabilityPromise;

                    Debug.LogFormat("VPS Availability at ({0}, {1}): {2}",
                        location.latitude, location.longitude, vpsAvailabilityPromise.Result);
                }

        IEnumerator StartLocationService()
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("Location service is disabled by the user.");
                HandleError(ErrorCode.LocationServiceDisabled);
                yield break;
            }

            Debug.Log("Starting location service.");
            Input.location.Start();

            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                yield return null;
            }

            if (Input.location.status != LocationServiceStatus.Running)
            {
                Debug.LogWarningFormat("Location service ended with {0} status.", Input.location.status);
                Input.location.Stop();
                HandleError(ErrorCode.LocationServiceNotRunning);
            }
        }

        void LifecycleUpdate()
        {
            // Only allow the screen to sleep when not tracking.
            int sleepTimeout = SleepTimeout.NeverSleep;
            if (ARSession.state != ARSessionState.SessionTracking)
            {
                sleepTimeout = SleepTimeout.SystemSetting;
            }

            Screen.sleepTimeout = sleepTimeout;

            // Quit the app if ARSession is in an error status.
            ErrorCode reason;
            if (ARSession.state is
                not ARSessionState.CheckingAvailability and
                not ARSessionState.Ready and
                not ARSessionState.SessionInitializing and
                not ARSessionState.SessionTracking)
            {
                reason = ErrorCode.ARSessionErrorState;
            }
            else if (Input.location.status == LocationServiceStatus.Failed)
            {
                reason = ErrorCode.LocationServiceFailed;
            }
            else if (m_SessionOrigin == null || m_Session == null || m_ARCoreExtensions == null)
            {
                reason = ErrorCode.MissingARComponents;
            }
            else
            {
                return;
            }

            HandleError(reason);
        }

        void HandleError(ErrorCode reason)
        {
            Debug.LogError($"GeospatialController Error: {reason}");
            m_HasError = true;
            OnError?.Invoke(reason);
        }
    }
}