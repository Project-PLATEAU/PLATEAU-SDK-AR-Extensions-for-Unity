using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// Menu UI component.
    /// </summary>
    public class ARMenuUI : MonoBehaviour
    {
        /// <summary>
        /// An entity of a menu.
        /// </summary>
        [Serializable]
        class MenuButton
        {
            public Button m_Button;
            public GameObject m_Target;
        }

        [SerializeField] MenuButton[] m_MenuButtons;

        void Start()
        {
            foreach (MenuButton menuButton in m_MenuButtons)
            {
                menuButton.m_Target.gameObject.SetActive(false);
                menuButton.m_Button.onClick.AddListener(() =>
                {
                    if (menuButton.m_Target.gameObject.activeSelf)
                    {
                        menuButton.m_Target.gameObject.SetActive(false);
                        return;
                    }

                    foreach (MenuButton other in m_MenuButtons)
                    {
                        other.m_Target.gameObject.SetActive(false);
                    }
                    menuButton.m_Target.gameObject.SetActive(true);
                });
            }
        }
    }
}