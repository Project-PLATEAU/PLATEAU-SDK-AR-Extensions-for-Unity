using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlateauToolkit.AR
{
    /// <summary>
    /// City Models based on AR marker positioning.
    /// </summary>
    public class PlateauARMarkerCityModel : MonoBehaviour
    {
        [SerializeField] GameObject m_CityModel;

        [Serializable]
        class ARMarkerConfiguration
        {
            /// <summary>
            /// The Guid of the target reference image in <see cref="IReferenceImageLibrary" />.
            /// </summary>
            [SerializeField] string m_TargetImageGuid;

            /// <summary>
            /// The transform of the AR marker.
            /// </summary>
            [SerializeField] Transform m_MarkerPoint;

            Material m_GizmoMaterial;

            public string TargetImageGuid => m_TargetImageGuid;
            public Transform MarkerPoint => m_MarkerPoint;

#if UNITY_EDITOR
            public Material GizmoMaterial
            {
                get => m_GizmoMaterial;
                set => m_GizmoMaterial = value;
            }
#endif
        }

        [SerializeField] ARMarkerConfiguration[] m_ARMarkerConfigurations;

        /// <summary>
        /// <see cref="ARTrackedImageManager" /> Reference.
        /// </summary>
        /// <remarks>
        /// <see cref="PlateauARMarkerCityModel" /> subscribes tracked image events.
        /// </remarks>
        [SerializeField] ARTrackedImageManager m_ARTrackedImageManager;

        /// <summary>
        /// The current tracked image.
        /// </summary>
        /// <remarks>
        /// The reference will be set when an AR marker is tracked by <see cref="ARTrackedImageManager" />.
        /// </remarks>
        [CanBeNull] ARTrackedImage m_CurrentTrackedImage;

        [SerializeField] [CanBeNull] ARMarkerConfiguration m_CurrentConfiguration;

        [SerializeField] Vector3 m_ToPosition;
        [SerializeField] Quaternion m_ToRotation;

        /// <summary>
        /// The current status of AR marker tracking.
        /// </summary>
        /// <returns>
        /// returns null if no AR marker is tracked.
        /// </returns>
        public TrackingState? CurrentTrackingStatus
        {
            get
            {
                if (m_CurrentTrackedImage == null)
                {
                    return null;
                }

                return m_CurrentTrackedImage.trackingState;
            }
        }

        void Awake()
        {
            // Disable the city model by default.
            m_CityModel.SetActive(false);

            m_ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDestroy()
        {
            m_ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
        {
            foreach (ARTrackedImage addedImage in args.added)
            {
                foreach (ARMarkerConfiguration configuration in m_ARMarkerConfigurations)
                {
                    if (addedImage.referenceImage.guid.ToString() == configuration.TargetImageGuid)
                    {
                        m_CityModel.SetActive(true);
                        m_CurrentConfiguration = configuration;
                        SetARTrackedImage(addedImage);
                        break;
                    }
                }
            }

            foreach (ARTrackedImage updatedImage in args.updated)
            {
                foreach (ARMarkerConfiguration configuration in m_ARMarkerConfigurations)
                {
                    if (updatedImage.referenceImage.guid.ToString() == configuration.TargetImageGuid)
                    {
                        m_CityModel.SetActive(true);
                        m_CurrentConfiguration = configuration;
                        SetARTrackedImage(updatedImage);
                        break;
                    }
                }
            }

            foreach (ARTrackedImage removedImage in args.removed)
            {
                if (m_CurrentTrackedImage != null && removedImage.trackableId == m_CurrentTrackedImage.trackableId)
                {
                    m_CurrentTrackedImage = null;
                    m_CurrentConfiguration = null;
                    m_CityModel.SetActive(false);
                }
            }
        }

        void SetARTrackedImage(ARTrackedImage trackedImage)
        {
            if (m_CurrentConfiguration == null)
            {
                Debug.LogError("Current Configuration is null");
                return;
            }

            Transform cityModelTransform = transform;
            m_CurrentTrackedImage = trackedImage;

            Vector3 offset = m_CurrentConfiguration.MarkerPoint.position - cityModelTransform.position;
            m_ToPosition = trackedImage.transform.position - offset;

            Quaternion rotationOffset = Quaternion.Inverse(cityModelTransform.rotation) * m_CurrentConfiguration.MarkerPoint.rotation;
            m_ToRotation = trackedImage.transform.rotation * Quaternion.Inverse(rotationOffset);
        }

        void Update()
        {
            Transform cityModelTransform = transform;

            // Interpolate to the position and the rotation of the city model calculated from the tracked image.
            cityModelTransform.position = Vector3.Lerp(cityModelTransform.position, m_ToPosition, Time.deltaTime * 10);
            cityModelTransform.rotation = Quaternion.Lerp(cityModelTransform.rotation, m_ToRotation, Time.deltaTime * 10);
        }

#if UNITY_EDITOR
        const float k_ARMarkerGizmoWidth = 0.4f;
        const float k_ARMarkerGizmoHeight = 0.4f;

        // TODO: The path should support the situation that the toolkit is installed in Assets?
        const string k_ARMarkerGizmoIconPath =
            "Packages/com.unity.plateautoolkit.ar/PlateauToolkit.AR/Editor/Gizmos/GizmoARMarkerIcon.png";

        Texture GetTargetARMarkerTexture(ARMarkerConfiguration configuration)
        {
            for (int i = 0; i < m_ARTrackedImageManager.referenceLibrary.count; i++)
            {
                XRReferenceImage referenceImage = m_ARTrackedImageManager.referenceLibrary[i];
                if (referenceImage.guid.ToString() == configuration.TargetImageGuid)
                {
                    string textureGuid = referenceImage.textureGuid.ToString("N");
                    string texturePath = AssetDatabase.GUIDToAssetPath(textureGuid);
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
                    return texture;
                }
            }

            return null;
        }

        void OnDrawGizmos()
        {
            if (m_ARTrackedImageManager == null || m_ARMarkerConfigurations == null)
            {
                return;
            }

            foreach (ARMarkerConfiguration configuration in m_ARMarkerConfigurations)
            {
                if (configuration.MarkerPoint == null)
                {
                    continue;
                }

                if (configuration.GizmoMaterial == null)
                {
                    Texture targetARMarkerTexture = GetTargetARMarkerTexture(configuration);
                    if (targetARMarkerTexture == null)
                    {
                        continue;
                    }

                    configuration.GizmoMaterial = new Material(Shader.Find("Unlit/GizmoARMarker"))
                    {
                        hideFlags = HideFlags.HideAndDontSave,
                        mainTexture = targetARMarkerTexture,
                    };
                }

                Mesh mesh = CreateQuad();
                configuration.GizmoMaterial.SetPass(0);
                Vector3 markerPosition = configuration.MarkerPoint.position;

                Graphics.DrawMeshNow(mesh, markerPosition, configuration.MarkerPoint.rotation, 0);
                DestroyImmediate(mesh);

                Gizmos.DrawIcon(markerPosition, k_ARMarkerGizmoIconPath);
            }
        }

        static Mesh CreateQuad()
        {
            Mesh mesh = new();

            Vector3[] vertices = {
                new (-k_ARMarkerGizmoWidth * 0.5f, 0, -k_ARMarkerGizmoHeight * 0.5f), // bottom-left
                new (k_ARMarkerGizmoWidth * 0.5f, 0, -k_ARMarkerGizmoHeight * 0.5f),  // bottom-right
                new (-k_ARMarkerGizmoWidth * 0.5f, 0, k_ARMarkerGizmoHeight * 0.5f),  // top-left
                new (k_ARMarkerGizmoWidth * 0.5f, 0, k_ARMarkerGizmoHeight * 0.5f),   // top-right
            };

            Vector2[] uv = {
                new (0, 0),
                new (1, 0),
                new (0, 1),
                new (1, 1),
            };

            int[] triangles = {
                2, 1, 0, // Swapped these
                3, 1, 2  // Swapped these
            };

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }
#endif
    }
}