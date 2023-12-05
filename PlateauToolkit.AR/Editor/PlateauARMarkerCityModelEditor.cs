using PlateauToolkit.Editor;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace PlateauToolkit.AR.Editor
{
    [CustomEditor(typeof(PlateauARMarkerCityModel))]
    public class PlateauARMarkerCityModelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            PlateauToolkitEditorGUILayout.Header("コンポーネント設定");

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CityModel"), new GUIContent("都市モデルオブジェクト"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ARTrackedImageManager"), new GUIContent("マーカー画像ライブラリ"));

            serializedObject.ApplyModifiedProperties();

            var trackedImageManager = (ARTrackedImageManager)serializedObject.FindProperty("m_ARTrackedImageManager").objectReferenceValue;
            if (trackedImageManager == null)
            {
                return;
            }

            PlateauToolkitEditorGUILayout.Header("ARマーカー設定");

            bool isDirty = false;

            int deletedIndex = -1;
            SerializedProperty markerConfigurationsProperty = serializedObject.FindProperty("m_ARMarkerConfigurations");
            for (int i = 0; i < markerConfigurationsProperty.arraySize; i++)
            {
                SerializedProperty markerConfigurationProperty = markerConfigurationsProperty.GetArrayElementAtIndex(i);

                IEnumerator fieldEnumerator = markerConfigurationProperty.GetEnumerator();
                fieldEnumerator.MoveNext();
                SerializedProperty arMarkerGuidProperty = ((SerializedProperty)fieldEnumerator.Current)?.Copy();
                Debug.Assert(arMarkerGuidProperty != null);
                fieldEnumerator.MoveNext();
                SerializedProperty arMarkerPointProperty = ((SerializedProperty)fieldEnumerator.Current)?.Copy();
                Debug.Assert(arMarkerPointProperty != null);

                DrawARMarkerConfiguration(
                    i, arMarkerGuidProperty, arMarkerPointProperty, trackedImageManager,
                    ref isDirty, ref deletedIndex);
            }

            if (deletedIndex >= 0)
            {
                markerConfigurationsProperty.DeleteArrayElementAtIndex(deletedIndex);
                isDirty = true;
            }

            if (GUILayout.Button("＋"))
            {
                markerConfigurationsProperty.InsertArrayElementAtIndex(markerConfigurationsProperty.arraySize);
                isDirty = true;
            }

            if (isDirty)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        void DrawARMarkerConfiguration(
            int index,
            SerializedProperty targetImageGuidProperty,
            SerializedProperty targetMarkerPointProperty,
            ARTrackedImageManager trackedImageManager,
            ref bool isDirty, ref int deletedIndex)
        {
            int selectedIndex = 0;
            var options = new GUIContent[trackedImageManager.referenceLibrary.count + 1];
            options[0] = new("-");
            for (int i = 0; i < options.Length - 1; i++)
            {
                XRReferenceImage referenceImage = trackedImageManager.referenceLibrary[i];
                options[i + 1] = new(referenceImage.name);

                if (referenceImage.guid.ToString() == targetImageGuidProperty.stringValue)
                {
                    selectedIndex = i + 1;
                }
            }

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                using (PlateauToolkitEditorGUILayout.BackgroundColorScope(Color.red))
                {
                    if (GUILayout.Button("✗", GUILayout.Width(24), GUILayout.Height(24)))
                    {
                        deletedIndex = index;
                    }
                }

                GUILayout.Space(6);

                int nextIndex;
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField($"マーカー設定 ({index})", EditorStyles.label);
                    nextIndex = EditorGUILayout.Popup(selectedIndex, options);
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(targetMarkerPointProperty, GUIContent.none);
                    isDirty |= EditorGUI.EndChangeCheck();
                }

                if (nextIndex != selectedIndex)
                {
                    if (nextIndex == 0)
                    {
                        targetImageGuidProperty.stringValue = null;
                    }
                    else
                    {
                        targetImageGuidProperty.stringValue =
                            trackedImageManager.referenceLibrary[nextIndex - 1].guid.ToString();
                    }

                    isDirty = true;
                }

                if (nextIndex > 0)
                {
                    int referenceImageIndex = nextIndex - 1;
                    string textureGuid = trackedImageManager.referenceLibrary[referenceImageIndex].textureGuid
                        .ToString("N");
                    string texturePath = AssetDatabase.GUIDToAssetPath(textureGuid);
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);

                    GUILayout.Space(6);
                    Rect logoRect = EditorGUILayout.GetControlRect(GUILayout.Width(64), GUILayout.Height(64));
                    GUI.DrawTexture(logoRect, texture);
                }
            }
        }
    }
}