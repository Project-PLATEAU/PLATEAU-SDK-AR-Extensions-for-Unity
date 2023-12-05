// <copyright file="SafeAreaScaler.cs" company="Google LLC">
//
// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
// PLATEAU SDK AR Extensions for Unity
//
// Modified by: Shohei Miyashita (Unity Technologies Japan)
// Date: 2023/09/22
// Changes:
// - Applied the naming rules and coding standards of PLATEAU AR Toolkit.

using UnityEngine;

namespace PlateauAR.Geospatial
{
    /// <summary>
    /// A helper component that scale the UI rect to the same size as the safe area.
    /// </summary>
    public class SafeAreaScaler : MonoBehaviour
    {
        RectTransform m_RectTransform;
        Rect m_LastSafeArea;

        void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_LastSafeArea = Screen.safeArea;
            ApplySafeArea();
        }

        void Update()
        {
            if (m_LastSafeArea != Screen.safeArea)
            {
                m_LastSafeArea = Screen.safeArea;
                ApplySafeArea();
            }
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            m_RectTransform.anchorMin = anchorMin;
            m_RectTransform.anchorMax = anchorMax;
        }
    }
}
