using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using PLATEAU.CityInfo;
using PlateauToolkit.Editor;

namespace PlateauToolkit.AR.Editor
{
    /// <summary>
    /// PLATEAU SDK AR Extensions for Unity Toolkit Window
    /// </summary>
    class PlateauARWindow : EditorWindow
    {
        PlateauARWindow m_Window;
        Material m_ChangeMaterial;

        void OnGUI()
        {
            if (m_Window == null)
            {
                m_Window = GetWindow<PlateauARWindow>();
            }

            PlateauToolkitEditorGUILayout.HeaderLogo(m_Window.position.width);

            PlateauToolkitEditorGUILayout.Header("AR");

            EditorGUILayout.LabelField("マテリアル設定", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "PLATEAU SDKでインポートされた3D都市モデルのマテリアルを一括変更します。",
                MessageType.Info);

            m_ChangeMaterial = (Material)EditorGUILayout.ObjectField("マテリアル", m_ChangeMaterial, typeof(Material), false);

            if (GUILayout.Button("ARオクルージョン遮蔽用マテリアルの参照を取得"))
            {
                string[] assetGuids = AssetDatabase.FindAssets("ZWrite", new[] { "Packages/com.unity.plateautoolkit.ar" });
                string zWriteMaterialPath = null;
                foreach (string guid in assetGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (path.EndsWith(".mat"))
                    {
                        zWriteMaterialPath = path;
                        break;
                    }
                }

                if (zWriteMaterialPath == null)
                {
                    EditorUtility.DisplayDialog("エラー", "ZWriteマテリアルが見つかりません。PLATEAU AR Toolkitが正しくインストールされていることを確認してください。", "OK");
                }
                else
                {
                    var zWriteMaterial = (Material)AssetDatabase.LoadAssetAtPath(zWriteMaterialPath, typeof(Material));
                    m_ChangeMaterial = zWriteMaterial;
                }
            }

            if (GUILayout.Button("シーン上の都市モデルのマテリアルを変更"))
            {
                PLATEAUInstancedCityModel[] cityModels = FindObjectsByType<PLATEAUInstancedCityModel>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None);

                foreach (PLATEAUInstancedCityModel cityModel in cityModels)
                {
                    foreach (Renderer renderer in cityModel.GetComponentsInChildren<Renderer>())
                    {
                        var materials = new Material[renderer.sharedMaterials.Length];
                        for (int i = 0; i < materials.Length; i++)
                        {
                            materials[i] = m_ChangeMaterial;
                        }
                        renderer.sharedMaterials = materials;
                    }
                }
            }
        }
    }
}
