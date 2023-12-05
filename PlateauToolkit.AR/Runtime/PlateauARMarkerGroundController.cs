using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace PlateauToolkit.AR
{
    /// <summary>
    /// States of AR ground markers.
    /// </summary>
    public enum PlateauARGroundMarkerState
    {
        /// <summary>
        /// Marker hasn't been not detected.
        /// </summary>
        MarkerNotDetected,

        /// <summary>
        /// The building close to the detected marker hasn't been found.
        /// </summary>
        BuildingNotDetected,

        /// <summary>
        /// The AR marker and the nearest building have been found.
        /// </summary>
        Detected,
    }

    /// <summary>
    /// Controls the AR ground marker system.
    /// </summary>
    [RequireComponent(typeof(PlateauARPositioning))]
    public class PlateauARMarkerGroundController : MonoBehaviour
    {
        [SerializeField] LayerMask m_BuildingLayer;
        [SerializeField] ARTrackedImageManager m_TrackedImageManager;

        PlateauARPositioning m_ARPositioning;

        Vector3? m_TrackedImagePosition;
        Vector3? m_DetectedBottom;

        /// <summary>
        /// The gap between the actual building and its virtual building representation.
        /// </summary>
        public float HeightGap { get; private set; }

        /// <summary>
        /// The current state of AR ground marker controlling.
        /// </summary>
        public PlateauARGroundMarkerState MarkerState
        {
            get
            {
                if (m_TrackedImagePosition == null)
                {
                    return PlateauARGroundMarkerState.MarkerNotDetected;
                }
                if (m_DetectedBottom == null)
                {
                    return PlateauARGroundMarkerState.BuildingNotDetected;
                }

                return PlateauARGroundMarkerState.Detected;
            }
        }

        void Awake()
        {
            m_ARPositioning = GetComponent<PlateauARPositioning>();
            Debug.Assert(m_ARPositioning != null);
        }

        void Start()
        {
            m_TrackedImageManager.trackedImagesChanged += eventArgs =>
            {
                if (!m_ARPositioning.IsInitialized)
                {
                    return;
                }

                if (eventArgs.removed.Count != 0)
                {
                    m_TrackedImagePosition = null;
                }

                ARTrackedImage trackedImage;
                if (eventArgs.added.Count == 1)
                {
                    trackedImage = eventArgs.added[0];
                }
                else if (eventArgs.updated.Count == 1)
                {
                    trackedImage = eventArgs.updated[0];
                }
                else if (eventArgs.added.Count > 1 || eventArgs.updated.Count > 1)
                {
                    Debug.LogError($"2 more events: added {eventArgs.added.Count}, updated: {eventArgs.updated.Count}");
                    return;
                }
                else
                {
                    return;
                }

                m_TrackedImagePosition = trackedImage.transform.position;
                m_DetectedBottom = null;
            };
        }

        void Update()
        {
            if (m_TrackedImagePosition != null)
            {
                if (m_DetectedBottom == null)
                {
                    if (FindNearestBuilding(m_TrackedImagePosition.Value, out Vector3 bottom))
                    {
                        m_DetectedBottom = bottom;
                    }
                    else
                    {
                        m_DetectedBottom = null;
                        return;
                    }
                }

                if (m_DetectedBottom != null)
                {
                    HeightGap = Vector3.Dot(m_DetectedBottom.Value - m_TrackedImagePosition.Value, m_ARPositioning.transform.up);
                }
            }
        }

        const float k_FindNearestBuildingRadiusScale = 0.5f;
        const int k_FindNearestBuildingAttempts = 100;
        const float k_FindNearestBuildingBase = 100;
        const float k_FindNearestBuildingDistance = 500;

        /// <summary>
        /// Find the nearest building around <see cref="point" /> by raycasting.
        /// </summary>
        /// <remarks>
        /// Starting a small raycast box, find the nearest point
        /// by increasing the size of the raycasting box.
        /// </remarks>
        bool FindNearestBuilding(in Vector3 point, out Vector3 bottom)
        {
            for (int i = 0; i < k_FindNearestBuildingAttempts; i++)
            {
                float radius = (i + 1) * k_FindNearestBuildingRadiusScale;
                Vector3 origin = point;
                origin += -m_ARPositioning.transform.up * k_FindNearestBuildingBase;

                if (Physics.BoxCast(
                        origin, new Vector3(radius, 0.5f, radius), m_ARPositioning.transform.up,
                        out RaycastHit hit,
                        Quaternion.identity, k_FindNearestBuildingDistance, m_BuildingLayer))
                {
                    bottom = hit.point;
                    return true;
                }
            }

            bottom = Vector3.zero;
            return false;
        }
    }
}