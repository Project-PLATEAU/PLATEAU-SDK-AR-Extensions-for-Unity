using UnityEngine;
using Google.XR.ARCoreExtensions;

namespace PlateauToolkit.AR
{
    /// <summary>
    /// The interface of Geospatial API controller.
    /// </summary>
    /// <remarks>
    /// Implementation of controllers for Geospatial API depends on the application where it runs,
    /// then <see cref="PlateauToolkit.AR" /> doesn't have any concrete implementation for that.
    /// </remarks>
    public class PlateauARGeospatialController : MonoBehaviour
    {
        /// <summary>
        /// If Geospatial API is ready.
        /// </summary>
        public virtual bool IsReady()
        {
            return false;
        }

        public virtual bool HasError()
        {
            return false;
        }

        /// <summary>
        /// Try to get Geospatial pose.
        /// </summary>
        public virtual bool TryGetPose(out GeospatialPose pose)
        {
            pose = new GeospatialPose();
            return false;
        }

        /// <summary>
        /// Create an anchor of Geospatial API and parent it to the <see cref="obj" />
        /// </summary>
        /// <param name="latitude">The latitude of the anchor</param>
        /// <param name="longitude">The longitude of the anchor</param>
        /// <param name="altitude">The altitude of the anchor</param>
        /// <param name="obj">A game object that will be child of the created anchor</param>
        public virtual void CreateAnchoredObject(double latitude, double longitude, double altitude, Transform obj)
        {
        }
    }
}