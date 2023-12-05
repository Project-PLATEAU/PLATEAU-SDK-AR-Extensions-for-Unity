using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// Color input UI component.
    /// </summary>
    public class ColorInputUI : MonoBehaviour
    {
        [SerializeField] ColorSliderUI m_RSlider;
        [SerializeField] ColorSliderUI m_GSlider;
        [SerializeField] ColorSliderUI m_BSlider;
        [SerializeField] ColorSliderUI m_ASlider;

        [SerializeField] Image m_ColorPreview;

        public Color Value
        {
            get => new(m_RSlider.Value, m_GSlider.Value, m_BSlider.Value, m_ASlider.Value);
            set
            {
                m_ColorPreview.color = new Color(value.r, value.g, value.b, value.a);
                m_RSlider.Value = value.r;
                m_GSlider.Value = value.g;
                m_BSlider.Value = value.b;
                m_ASlider.Value = value.a;
            }
        }

        /// <summary>
        /// Events when the color value is changed.
        /// </summary>
        public event Action<Color> OnColorChanged;

        void OnEnable()
        {
            m_RSlider.onValueChanged += OnColorValueChanged;
            m_GSlider.onValueChanged += OnColorValueChanged;
            m_BSlider.onValueChanged += OnColorValueChanged;
            m_ASlider.onValueChanged += OnColorValueChanged;
        }

        void OnColorValueChanged(float _)
        {
            m_ColorPreview.color = Value;
            OnColorChanged?.Invoke(Value);
        }
    }
}