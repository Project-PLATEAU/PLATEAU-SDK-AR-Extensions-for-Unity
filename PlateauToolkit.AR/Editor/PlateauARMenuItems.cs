using UnityEditor;

namespace PlateauToolkit.AR.Editor
{
    /// <summary>
    /// Editor menus for PLATEAU SDK AR Extensions for Unity
    /// </summary>
    static class PlateauARMenuItems
    {
        [MenuItem("PLATEAU/PLATEAU Toolkit/AR Toolkit", priority = 0)]
        static void ShowSandboxWindow()
        {
            EditorWindow.GetWindow(typeof(PlateauARWindow), false, "PLATEAU AR Toolkit");
        }
    }
}