using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlateauAR
{
    /// <summary>
    /// A boot tool for AR sample
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] Button m_ButtonPrefab;
        [SerializeField] RectTransform m_ButtonContainer;

        void Start()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                Button button = Instantiate(m_ButtonPrefab, m_ButtonContainer, false);
                button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(sceneName);
                });

                button.transform.GetChild(0).GetComponent<TMP_Text>().text = scenePath;
            }

            Destroy(m_ButtonPrefab.gameObject);
        }
    }
}