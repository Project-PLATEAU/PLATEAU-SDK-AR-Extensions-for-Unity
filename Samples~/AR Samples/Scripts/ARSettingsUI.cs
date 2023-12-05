using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// UI components for the settings of AR
    /// </summary>
    public class ARSettingsUI : MonoBehaviour
    {
        [SerializeField] Vector3InputUI m_OffsetInputUI;
        [SerializeField] AROffsetControllerUI m_OffsetControllerUI;

        [SerializeField] ARGroundMarkerUI m_ARGroundMarkerUI;

        [SerializeField] Image m_3DTileHeaderLine;
        [SerializeField] TMPro.TMP_Text m_3DTileHeader;
        [SerializeField] TMPro.TMP_Dropdown m_PrefectureDropdown;
        [SerializeField] TMPro.TMP_Dropdown m_StreamingUrlDropdown;

        [SerializeField] TMPro.TMP_Dropdown m_MaterialDropdown;

        [SerializeField] ColorInputUI m_ColorInput;

        [SerializeField] TMPro.TMP_Dropdown m_PlaybackDropdown;
        [SerializeField] Button m_StartPlaybackButton;
        [SerializeField] Button m_DeletePlaybackButton;
        [SerializeField] Button m_StartRecordingButton;

        public Vector3InputUI OffsetInputUI => m_OffsetInputUI;
        public AROffsetControllerUI OffsetControllerUI => m_OffsetControllerUI;
        public ARGroundMarkerUI ARGroundMarkerUI => m_ARGroundMarkerUI;
        public TMPro.TMP_Dropdown PrefectureDropdown => m_PrefectureDropdown;
        public TMPro.TMP_Dropdown StreamingUrlDropdown => m_StreamingUrlDropdown;
        public TMPro.TMP_Dropdown MaterialDropdown => m_MaterialDropdown;
        public ColorInputUI ColorInput => m_ColorInput;

        public TMPro.TMP_Dropdown PlaybackDropdown => m_PlaybackDropdown;
        public Button StartPlaybackButton => m_StartPlaybackButton;
        public Button DeletePlaybackButton => m_DeletePlaybackButton;
        public Button StartRecordingButton => m_StartRecordingButton;


        public void Set3DTileUIEnable(bool enable)
        {
            m_3DTileHeader.gameObject.SetActive(enable);
            m_StreamingUrlDropdown.gameObject.SetActive(enable);
            m_PrefectureDropdown.gameObject.SetActive(enable);
            m_3DTileHeaderLine.gameObject.SetActive(enable);
        }
    }
}