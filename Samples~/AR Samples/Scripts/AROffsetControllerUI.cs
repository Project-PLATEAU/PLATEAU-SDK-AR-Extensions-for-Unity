using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// UI for changing the offset.
    /// </summary>
    public class AROffsetControllerUI : MonoBehaviour
    {
        [Serializable]
        class OffsetController
        {
            public Button m_XUpButton;
            public Button m_XDownButton;
            public Button m_YUpButton;
            public Button m_YDownButton;
            public Button m_ZUpButton;
            public Button m_ZDownButton;
        }

        [FormerlySerializedAs("m_10cmOffsetController")] [SerializeField] OffsetController m_10CmOffsetController;
        [FormerlySerializedAs("m_1cmOffsetController")] [SerializeField] OffsetController m_1CmOffsetController;

        /// <summary>
        /// Events when the offset value is changed
        /// </summary>
        public event Action<Vector3> OnOffsetChanged;

        void Start()
        {
            const float v10Cm = 0.1f;
            SetUpButton(m_10CmOffsetController.m_XUpButton, new Vector3(v10Cm, 0, 0));
            SetUpButton(m_10CmOffsetController.m_XDownButton, new Vector3(-v10Cm, 0, 0));
            SetUpButton(m_10CmOffsetController.m_YUpButton, new Vector3(0, v10Cm, 0));
            SetUpButton(m_10CmOffsetController.m_YDownButton, new Vector3(0, -v10Cm, 0));
            SetUpButton(m_10CmOffsetController.m_ZUpButton, new Vector3(0, 0, v10Cm));
            SetUpButton(m_10CmOffsetController.m_ZDownButton, new Vector3(0, 0, -v10Cm));

            const float v1Cm = 0.01f;
            SetUpButton(m_1CmOffsetController.m_XUpButton, new Vector3(v1Cm, 0, 0));
            SetUpButton(m_1CmOffsetController.m_XDownButton, new Vector3(-v1Cm, 0, 0));
            SetUpButton(m_1CmOffsetController.m_YUpButton, new Vector3(0, v1Cm, 0));
            SetUpButton(m_1CmOffsetController.m_YDownButton, new Vector3(0, -v1Cm, 0));
            SetUpButton(m_1CmOffsetController.m_ZUpButton, new Vector3(0, 0, v1Cm));
            SetUpButton(m_1CmOffsetController.m_ZDownButton, new Vector3(0, 0, -v1Cm));
        }

        void SetUpButton(Button button, Vector3 offsetDelta)
        {
            button.onClick.AddListener(() =>
            {
                OnOffsetChanged?.Invoke(offsetDelta);
            });
        }
    }
}