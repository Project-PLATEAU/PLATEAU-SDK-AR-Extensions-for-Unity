using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor
{
    const string k_ShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
    const string k_ReadmeSourceDirectory = "Assets/TutorialInfo";
    const float k_Space = 16f;

    static ReadmeEditor()
    {
        EditorApplication.delayCall += SelectReadmeAutomatically;
    }

    static void RemoveTutorial()
    {
        if (EditorUtility.DisplayDialog("READMEアセットの削除",
            $"{k_ReadmeSourceDirectory}に含まれるREADMEアセットを削除しますか？この操作は取り消せません。",
            "はい",
            "キャンセル"))
        {
            if (Directory.Exists(k_ReadmeSourceDirectory))
            {
                FileUtil.DeleteFileOrDirectory(k_ReadmeSourceDirectory);
                FileUtil.DeleteFileOrDirectory(k_ReadmeSourceDirectory + ".meta");
            }
            else
            {
                Debug.Log($"READMEアセットディレクトリ（{k_ReadmeSourceDirectory}）が見つかりませんでした。");
            }

            Readme readmeAsset = SelectReadme();
            if (readmeAsset != null)
            {
                string path = AssetDatabase.GetAssetPath(readmeAsset);
                FileUtil.DeleteFileOrDirectory(path + ".meta");
                FileUtil.DeleteFileOrDirectory(path);
            }

            AssetDatabase.Refresh();
        }
    }

    static void SelectReadmeAutomatically()
    {
        if (!SessionState.GetBool(k_ShowedReadmeSessionStateName, false))
        {
            Readme readme = SelectReadme();
            SessionState.SetBool(k_ShowedReadmeSessionStateName, true);

            if (readme && !readme.m_LoadedLayout)
            {
                LoadLayout();
                readme.m_LoadedLayout = true;
            }
        }
    }

    static void LoadLayout()
    {
        Assembly assembly = typeof(EditorApplication).Assembly;
        Type windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
        MethodInfo method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
        if (method != null)
        {
            method.Invoke(null, new object[] { Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false });
        }
        else
        {
            Debug.Log("レイアウトファイルの読み込みに失敗しました");
        }
    }

    static Readme SelectReadme()
    {
        string[] ids = AssetDatabase.FindAssets("Readme t:Readme");
        if (ids.Length == 1)
        {
            Object readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

            Selection.objects = new[] { readmeObject };

            return (Readme)readmeObject;
        }

        Debug.Log("READMEが見つかりません。");
        return null;
    }

    protected override void OnHeaderGUI()
    {
        var readme = (Readme)target;
        Init();

        float iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

        GUILayout.BeginHorizontal("In BigTitle");
        {
            if (readme.m_Icon != null)
            {
                GUILayout.Space(k_Space);
                GUILayout.Label(readme.m_Icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
            }
            GUILayout.Space(k_Space);
            GUILayout.BeginVertical();
            {

                GUILayout.FlexibleSpace();
                GUILayout.Label(readme.m_Title, TitleStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        var readme = (Readme)target;
        Init();

        foreach (Readme.Section section in readme.m_Sections)
        {
            GUILayout.Space(k_Space);

            if (!string.IsNullOrEmpty(section.m_Heading))
            {
                GUILayout.Label(section.m_Heading, HeadingStyle);
            }

            if (!string.IsNullOrEmpty(section.m_Text))
            {
                GUILayout.Label(section.m_Text, BodyStyle);
            }

            if (!string.IsNullOrEmpty(section.m_LinkText))
            {
                if (LinkLabel(new GUIContent(section.m_LinkText)))
                {
                    Application.OpenURL(section.m_URL);
                }
            }
        }

        GUILayout.Space(k_Space);

        GUILayout.Label("このREADMEが不要な場合は以下のボタンからREADMEに関するアセットを削除することができます。", BodyStyle);
        if (GUILayout.Button("READMEアセットを削除", ButtonStyle))
        {
            RemoveTutorial();
        }
    }

    bool m_Initialized;

    GUIStyle LinkStyle => m_LinkStyle;

    [SerializeField] GUIStyle m_LinkStyle;

    GUIStyle TitleStyle => m_TitleStyle;

    [SerializeField] GUIStyle m_TitleStyle;

    GUIStyle HeadingStyle => m_HeadingStyle;

    [SerializeField] GUIStyle m_HeadingStyle;

    GUIStyle BodyStyle => m_BodyStyle;

    [SerializeField] GUIStyle m_BodyStyle;

    GUIStyle ButtonStyle => m_ButtonStyle;

    [SerializeField] GUIStyle m_ButtonStyle;

    void Init()
    {
        if (m_Initialized)
        {
            return;
        }

        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 14;
        m_BodyStyle.richText = true;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontStyle = FontStyle.Bold;
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;

        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        m_ButtonStyle = new GUIStyle(EditorStyles.miniButton);
        m_ButtonStyle.fontStyle = FontStyle.Bold;

        m_Initialized = true;
    }

    bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        Rect position = GUILayoutUtility.GetRect(label, LinkStyle, options);

        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, LinkStyle);
    }
}
