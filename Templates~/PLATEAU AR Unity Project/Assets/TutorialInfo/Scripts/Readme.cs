using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Readme : ScriptableObject
{
    [FormerlySerializedAs("icon")] public Texture2D m_Icon;
    [FormerlySerializedAs("title")] public string m_Title;
    [FormerlySerializedAs("sections")] public Section[] m_Sections;
    [FormerlySerializedAs("loadedLayout")] public bool m_LoadedLayout;

    [Serializable]
    public class Section
    {
        [FormerlySerializedAs("heading")] public string m_Heading;
        [FormerlySerializedAs("text")] public string m_Text;
        [FormerlySerializedAs("linkText")] public string m_LinkText;
        [FormerlySerializedAs("url")] public string m_URL;
    }
}
