namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="RiversCreator" />.
    /// </summary>
    public class RiversCreator : Creator
    {
        /// <summary>
        /// Defines the _riverPrefab.
        /// </summary>
        [SerializeField] protected GameObject _riverPrefab;

        /// <summary>
        /// Defines the idName.
        /// </summary>
        private const string idName = "rivers";

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public override void Create(dynamic collection, float scale, GameObject city)
        {
            var _parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Rivers);
            var riversFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            if (riversFeature == null)
            {
                return;
            }
            var polygons = CityGeneratorHelper.GetPolygonsFromFeature(riversFeature);
            foreach (var poly in polygons)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_riverPrefab, _parent);
                MultiLineStringHandler.UpdateLineRendererPoints(poly, out positions, scale, lineRenderer);
            }
        }
    }
}
