using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// Color slider UI component.
    /// </summary>
    public class ColorSliderUI : MonoBehaviour
    {
        [SerializeField] Slider m_Slider;
        [SerializeField] TMPro.TMP_InputField m_InputField;

        public float Value
        {
            get => m_Slider.value;
            set => m_Slider.value = value;
        }

        public event Action<float> onValueChanged;

        void Awake()
        {
            m_Slider.maxValue = 1;
            m_Slider.minValue = 0;

            m_InputField.contentType = TMPro.TMP_InputField.ContentType.DecimalNumber;
        }

        void OnEnable()
        {
            m_InputField.onValueChanged.AddListener(OnInputFieldChanged);
            m_Slider.onValueChanged.AddListener(OnSliderChanged);
        }

        void OnDisable()
        {
            m_InputField.onValueChanged.RemoveListener(OnInputFieldChanged);
            m_Slider.onValueChanged.RemoveListener(OnSliderChanged);
        }

        void OnInputFieldChanged(string value)
        {
            m_Slider.value = float.TryParse(value, out float intValue) ? intValue : 0;
            onValueChanged?.Invoke(Value);
        }

        void OnSliderChanged(float value)
        {
            m_InputField.text = value.ToString();
            onValueChanged?.Invoke(Value);
        }
    }
}