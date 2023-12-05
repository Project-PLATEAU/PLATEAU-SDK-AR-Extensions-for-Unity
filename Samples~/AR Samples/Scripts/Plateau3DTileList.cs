using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace PlateauAR
{
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable CommentTypo
        /// <summary>
        /// JSON data of 3DTile entities of PLATEAU.
        /// </summary>
        /// <example>
        /// <code>
        /// {
        ///   "name": "建築物モデル（千代田区）",
        ///   "pref": "東京都",
        ///   "pref_code": "13",
        ///   "city": "東京都23区",
        ///   "city_code": "13100",
        ///   "ward": "千代田区",
        ///   "ward_en": "chiyoda-ku",
        ///   "ward_code": "13101",
        ///   "type": "建築物モデル",
        ///   "type_en": "bldg",
        ///   "url": "https://assets.cms.plateau.reearth.io/assets/11/6d05db-ed47-4f88-b565-9eb385b1ebb0/13100_tokyo23-ku_2022_3dtiles%20_1_1_op_bldg_13101_chiyoda-ku_lod1/tileset.json",
        ///   "year": 2022,
        ///   "lod": "1"
        /// }
        /// </code>
        /// </example>
        // ReSharper restore CommentTypo
        [Serializable]
        public class Plateau3DTile
        {
            // ReSharper disable InconsistentNaming
            [UsedImplicitly][SerializeField] string name;
            [UsedImplicitly][SerializeField] string pref;
            [UsedImplicitly][SerializeField] string pref_code;
            [UsedImplicitly][SerializeField] string city;
            [UsedImplicitly][SerializeField] string city_code;
            [UsedImplicitly][SerializeField] string ward;
            [UsedImplicitly][SerializeField] string ward_en;
            [UsedImplicitly][SerializeField] string ward_code;
            [UsedImplicitly][SerializeField] string type;
            [UsedImplicitly][SerializeField] string type_en;
            [UsedImplicitly][SerializeField] string url;
            [UsedImplicitly][SerializeField] string year;
            [UsedImplicitly][SerializeField] string lod;
            // ReSharper restore InconsistentNaming

            int ToInt(string numberStr)
            {
                return int.TryParse(numberStr, out int number) ? number : -1;
            }

            public string Name => name;
            public int Year => ToInt(year);
            public string PrefectureName => pref;
            public int PrefectureCode => ToInt(pref_code);
            public string City => city;
            public int CityCode => ToInt(city_code);
            public string Ward => ward;
            public string WardEn => ward_en;
            public int WardCode => ToInt(ward_code);
            public string Type => type;
            public string TypeEn => type_en;
            public string Url => url;
            public int Lod => ToInt(lod);
        }
#pragma warning restore IDE1006 // Naming Styles

    /// <summary>
    /// A prefecture of 3DTile.
    /// </summary>
    public class Plateau3DTilePrefecture
    {
        public Plateau3DTilePrefecture(string prefectureName)
        {
            PrefectureName = prefectureName;
            Urls = new();
        }

        public string PrefectureName { get; }
        public List<Plateau3DTile> Urls { get; }
    }

    /// <summary>
    /// PLATEAU 3DTile data list helper.
    /// </summary>
    public static class Plateau3DTileList
    {
        const string k_3DTileListJsonUrl =
            "https://raw.githubusercontent.com/Project-PLATEAU/plateau-streaming-tutorial/main/3dtiles_url.json";

        /// <summary>
        /// Fetch the list of PLATEAU 3DTiles from the PLATEAU streaming GitHub repository.
        /// </summary>
        /// <returns></returns>
        public static async Task<Plateau3DTilePrefecture[]> Get3DTilePrefectures()
        {
            var request = UnityWebRequest.Get(k_3DTileListJsonUrl);
            UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone)
            {
                await Task.Yield();
            }

            if (request.error != null)
            {
                Debug.LogError(request.error);
                return Array.Empty<Plateau3DTilePrefecture>();
            }

            string listJson = request.downloadHandler.text;
            string wrappedJson = $"{{\"m_Data\":{listJson}}}";
            Plateau3DTileApiResult result = JsonUtility.FromJson<Plateau3DTileApiResult>(wrappedJson);

            Dictionary<int, Plateau3DTilePrefecture> prefectureDictionary = new();
            foreach (Plateau3DTile streamingUrl in result.m_Data)
            {
                if (streamingUrl.TypeEn != "bldg")
                {
                    continue;
                }

                if (!prefectureDictionary.TryGetValue(streamingUrl.PrefectureCode, out Plateau3DTilePrefecture prefecture))
                {
                    prefecture = new(streamingUrl.PrefectureName);
                    prefectureDictionary[streamingUrl.PrefectureCode] = prefecture;
                }

                prefecture.Urls.Add(streamingUrl);
            }

            return prefectureDictionary.Values.ToArray();
        }

#pragma warning disable IDE1006 // Naming Styles
        [Serializable]
        class Plateau3DTileApiResult
        {
            [UsedImplicitly] public Plateau3DTile[] m_Data;
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}