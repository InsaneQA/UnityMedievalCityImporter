namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="RoadCreator" />.
    /// </summary>
    public class RoadCreator : Creator
    {
        /// <summary>
        /// Defines the _roadPrefab.
        /// </summary>
        [SerializeField] protected GameObject _roadPrefab;

        /// <summary>
        /// Defines the idName.
        /// </summary>
        private const string idName = "roads";

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public override void Create(dynamic collection, float scale, GameObject city)
        {
            var _roadParent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Rivers);
            var roadFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            if (roadFeature == null)
            {
                return;
            }
            var polygons = CityGeneratorHelper.GetPolygonsFromFeature(roadFeature); //(roadFeature.Geometry).Coordinates;
            foreach (var poly in polygons)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_roadPrefab, _roadParent);
                MultiLineStringHandler.UpdateLineRendererPoints(poly, out positions, scale, lineRenderer);
            }
        }
    }
}
