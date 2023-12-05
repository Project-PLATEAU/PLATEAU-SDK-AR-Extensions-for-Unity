using Google.XR.ARCoreExtensions;
using PlateauAR.Geospatial;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace PlateauAR
{
    /// <summary>
    /// Updates the information of AR frameworks.
    /// </summary>
    public class ARInfoController : MonoBehaviour
    {
        [SerializeField] ARInfoUI m_InfoUI;
        [SerializeField] GeospatialController m_GeospatialController;
        [SerializeField] AREarthManager m_EarthManager;

        string m_GeospatialError;

        void Awake()
        {
            m_GeospatialController.OnError += code =>
            {
                switch (code)
                {
                    case GeospatialController.ErrorCode.PermissionDenied:
                        m_GeospatialError = "アプリケーションに必要な権限が拒否されました";
                        break;
                    case GeospatialController.ErrorCode.LocationServiceDisabled:
                    case GeospatialController.ErrorCode.LocationServiceNotRunning:
                    case GeospatialController.ErrorCode.LocationServiceFailed:
                        m_GeospatialError = "位置情報サービスが利用できません";
                        break;
                    case GeospatialController.ErrorCode.ARSessionErrorState:
                    case GeospatialController.ErrorCode.MissingARComponents:
                    case GeospatialController.ErrorCode.ARNotSupported:
                        m_GeospatialError = "ARエラーが発生しました";
                        break;
                    case GeospatialController.ErrorCode.LocalizationTimeout:
                        m_GeospatialError = "位置推定がタイムアウトしました";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(code), code, null);
                }
            };
        }

        void Update()
        {
            string info = "";
            if (m_GeospatialError != null)
            {
                info += m_GeospatialError + "\n\n";
            }

            info +=
$@"[AR基本情報]
ARセッション: {ARSession.state}
AR機能可否: {m_EarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled)}
位置情報サービス: {Input.location.status}
地形: {m_EarthManager.EarthState}
地形トラッキング: {m_EarthManager.EarthTrackingState}";

            if (m_EarthManager.EarthState == EarthState.Enabled &&
                m_EarthManager.EarthTrackingState == TrackingState.Tracking)
            {
                GeospatialPose pose = m_EarthManager.CameraGeospatialPose;

                info +=
$@"
[VPS]
緯度経度: {pose.Latitude}, {pose.Longitude}
高度: {pose.Altitude}
方角: {pose.EunRotation}
水平精度: {pose.HorizontalAccuracy}
垂直精度: {pose.VerticalAccuracy}
方角精度: {pose.OrientationYawAccuracy}";
            }
            else
            {
                info += "\nVPS待機中";
            }

            m_InfoUI.SetInfoText(info);
        }
    }
}