using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// AR ground marker controlling UI component.
    /// </summary>
    public class ARGroundMarkerUI : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text m_ARGroundInfoText;
        [SerializeField] Button m_ApplyButton;

        public TMPro.TMP_Text ARGroundInfoText => m_ARGroundInfoText;
        public Button ApplyButton => m_ApplyButton;
    }
}