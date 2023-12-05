using PlateauToolkit.AR;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace PlateauAR
{
    /// <summary>
    /// An implementation of <see cref="PlateauARGeoidHeightProvider"/>
    /// with GSI (国土地理院) geoid height API.
    /// </summary>
    /// <remarks>
    /// GSI geoid height API is not for use of production level applications.
    /// This API can't be used for many users at the same time due to the server spec.
    /// </remarks>
    public class GsiGeoidHeightProvider : PlateauARGeoidHeightProvider
    {
        const string k_ApiUrlFormat =
            "https://vldb.gsi.go.jp/sokuchi/surveycalc/geoid/calcgh/cgi/geoidcalc.pl?outputType=json&latitude={0}&longitude={1}";

        /// <summary>
        /// Get a value of geoid height through GSI API.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public override async Task<double> GetGeoidHeight(double latitude, double longitude)
        {
            string url = string.Format(k_ApiUrlFormat, latitude, longitude);
            var request = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone)
            {
                await Task.Yield();
            }
            if (request.error != null)
            {
                Debug.LogError(request.error);
                return 0;
            }

            GsiGeoidHeightApiResult geoidHeightResult = JsonUtility.FromJson<GsiGeoidHeightApiResult>(request.downloadHandler.text);
            return geoidHeightResult.GeoidHeight;
        }

#pragma warning disable IDE1006 // Naming Styles
        [Serializable]
        class GsiGeoidHeightApiResult
        {
            [Serializable]
            class OutputDataClass
            {
                // ReSharper disable once InconsistentNaming
                public float geoidHeight;
            }

            // ReSharper disable once InconsistentNaming
            [SerializeField] OutputDataClass OutputData;

            public float GeoidHeight => OutputData.geoidHeight;
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}