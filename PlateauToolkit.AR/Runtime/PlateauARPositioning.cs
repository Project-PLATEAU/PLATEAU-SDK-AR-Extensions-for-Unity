using UnityEngine;
using Google.XR.ARCoreExtensions;
using System.Collections;
using System;
using PLATEAU.CityInfo;
using PLATEAU.Native;
using CesiumForUnity;
using System.Threading.Tasks;

namespace PlateauToolkit.AR
{
    /// <summary>
    /// Types of positioning objects
    /// </summary>
    public enum PlateauARPositioningType
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Instantiated models imported by PLATEAU SDK
        /// </summary>
        InstancedCityModel,

        /// <summary>
        /// 3DTile city models managed by Cesium
        /// </summary>
        Cesium,
    }

    /// <summary>
    /// Locates virtual 3D city models in a real world.
    /// </summary>
    /// <remarks>
    /// The location will be calculated and estimated by Geospatial API (VPS).
    /// The position in the geographic coordinate is required to locate virtual objects.
    /// Especially for altitude, some implementation to obtain an altitude for a object
    /// with latitude and longitude (<see cref="PlateauARGeoidHeightProvider" />)
    /// </remarks>
    public class PlateauARPositioning : MonoBehaviour
    {
        [SerializeReference] PlateauARGeospatialController m_GeospatialController;
        [SerializeReference] PlateauARGeoidHeightProvider m_GeoidHeightProvider;

        [Header("PLATEAU SDK 3D都市モデル")]
        [SerializeField] PLATEAUInstancedCityModel m_PlateauCityModel;

        [Header("Cesium 3DTiles 3D都市モデル")]
        [SerializeField] CesiumGeoreference m_CesiumGeoreference;
        [SerializeField] Cesium3DTileset m_Cesium3DTileset;

        bool m_Initialized;

        public PlateauARPositioningType PositioningType { get; private set; }
        public bool IsInitialized => m_Initialized;

        void Awake()
        {
            if (m_PlateauCityModel != null)
            {
                PositioningType = PlateauARPositioningType.InstancedCityModel;
            }
            else if (m_CesiumGeoreference != null && m_Cesium3DTileset != null)
            {
                PositioningType = PlateauARPositioningType.Cesium;
            }
            else
            {
                Debug.LogError("位置合わせオブジェクトが正しく設定されていません");
                PositioningType = PlateauARPositioningType.Invalid;
            }
        }

        void Start()
        {
            if (PositioningType == PlateauARPositioningType.Invalid)
            {
                throw new InvalidOperationException("Configuration error");
            }

            StartCoroutine(Initialize());
        }

        /// <summary>
        /// Initialize the process of positioning.
        /// </summary>
        /// <returns></returns>
        IEnumerator Initialize()
        {
            Debug.Assert(PositioningType != PlateauARPositioningType.Invalid, "Fields are set properly");

            // Wait for Geospatial API ready
            Debug.Log("Geospatial APIの初期化を待機中...");
            while (!m_GeospatialController.IsReady())
            {
                if (m_GeospatialController.HasError())
                {
                    Debug.LogError("GeospatialControllerが利用できません");
                    yield break;
                }

                yield return null;
            }

            double latitude;
            double longitude;
            if (m_PlateauCityModel != null)
            {
                // If a PLATEAU city model is specified, the origin of the city model is the position selected to import.
                GeoCoordinate coords = m_PlateauCityModel.GeoReference.Unproject(new PlateauVector3d(0, 0, 0));
                latitude = coords.Latitude;
                longitude = coords.Longitude;
            }
            else if (m_CesiumGeoreference != null)
            {
                GeospatialPose pose;
                while (!m_GeospatialController.TryGetPose(out pose))
                {
                    yield return null;
                }
                latitude = pose.Latitude;
                longitude = pose.Longitude;

                m_CesiumGeoreference.latitude = latitude;
                m_CesiumGeoreference.longitude = longitude;
                m_CesiumGeoreference.height = 0; // This is a temporary value
            }
            else
            {
                Debug.LogError("位置合わせをする対象が選択されていません");
                yield break;
            }

            Debug.Log("都市モデルのジオイド高を取得中...");
            // Obtain the geoid height on the position of the placed model
            Task<double> getGeoidHeightTask = m_GeoidHeightProvider.GetGeoidHeight(latitude, longitude);
            while (!getGeoidHeightTask.IsCompleted)
            {
                yield return null;
            }
            if (!getGeoidHeightTask.IsCompletedSuccessfully)
            {
                Debug.LogError("ジオイド高の取得に失敗しました");
                yield break;
            }

            float geoidHeight = (float)getGeoidHeightTask.Result;

            if (m_CesiumGeoreference != null)
            {
                m_CesiumGeoreference.height = geoidHeight;
            }

            Debug.Log("アンカーを作成");
            // Create a Geospatial Anchor and parent this game object to the anchor,
            // then the map objects will be placed on the actual position of a real world
            m_GeospatialController.CreateAnchoredObject(latitude, longitude, geoidHeight, transform);

            // ARGeospatialAnchor anchor = m_AnchorManager.AddAnchor(latitude, longitude, geoidHeight, Quaternion.identity);
            // transform.parent = anchor.transform;

            Debug.Log("初期化完了");
            m_Initialized = true;
        }

        /// <summary>
        /// Set the 3DTile streaming URL to <see cref="Cesium3DTileset" />
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method is available only in Cesium positioning.
        /// </exception>
        public void Set3DTilesetUrl(string url)
        {
            if (PositioningType != PlateauARPositioningType.Cesium)
            {
                throw new InvalidOperationException();
            }
            m_Cesium3DTileset.url = url;
        }

        /// <summary>
        /// Set an offset to adjust the buildings to the actual position perfectly.
        /// </summary>
        /// <remarks>
        /// Geospatial API can locate the buildings in a real world,
        /// but the calculation won't be perfect and they admit certain errors.
        /// Then if users want to fit them to the position, they've got to adjust the offset manually.
        /// </remarks>
        /// <param name="offset"></param>
        /// <returns>return <c>false</c> if positioning is not initialized</returns>
        public void SetOffset(Vector3 offset)
        {
            if (!m_Initialized)
            {
                Debug.LogError("位置合わせの初期化が完了していません");
                return;
            }

            transform.localPosition = offset;
        }

        /// <summary>
        /// Get the offset of positioning.
        /// </summary>
        /// <returns>return <c>false</c> if positioning is not initialized</returns>
        public bool GetOffset(out Vector3 offset)
        {
            if (!m_Initialized)
            {
                Debug.LogError("位置合わせの初期化が完了していません");
                offset = Vector3.zero;
                return false;
            }

            offset = transform.localPosition;
            return true;
        }
    }
}