using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// Vector3 input UI component.
    /// </summary>
    public class Vector3InputUI : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_InputField m_X;
        [SerializeField] TMPro.TMP_InputField m_Y;
        [SerializeField] TMPro.TMP_InputField m_Z;
        [SerializeField] Button m_ApplyButton;

        public Button.ButtonClickedEvent OnApplied => m_ApplyButton.onClick;

        void Start()
        {
            m_X.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
            m_Y.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
            m_Z.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;

            m_X.text = "0";
            m_Y.text = "0";
            m_Z.text = "0";
        }

        public Vector3 Value
        {
            get => new(
                float.TryParse(m_X.text, out float x) ? x : 0,
                float.TryParse(m_Y.text, out float y) ? y : 0,
                float.TryParse(m_Z.text, out float z) ? z : 0);
            set
            {
                m_X.text = value.x.ToString(CultureInfo.InvariantCulture);
                m_Y.text = value.y.ToString(CultureInfo.InvariantCulture);
                m_Z.text = value.z.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}