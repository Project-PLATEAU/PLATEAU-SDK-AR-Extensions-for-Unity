using UnityEngine;

namespace PlateauAR
{
    /// <summary>
    /// AR information UI component.
    /// </summary>
    public class ARInfoUI : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text m_InfoText;

        public void SetInfoText(string info)
        {
            m_InfoText.text = info;
        }
    }
}