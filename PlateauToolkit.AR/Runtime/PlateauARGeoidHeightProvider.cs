using System.Threading.Tasks;
using UnityEngine;

namespace PlateauToolkit.AR
{
    /// <summary>
    /// The interface of a geoid height provider.
    /// </summary>
    /// <remarks>
    /// Implements a class inherited this which can provide geoid height from latitude and longitude.
    /// </remarks>
    public class PlateauARGeoidHeightProvider : MonoBehaviour
    {
        public virtual Task<double> GetGeoidHeight(double latitude, double longitude)
        {
            return Task.FromResult(0.0);
        }
    }
}