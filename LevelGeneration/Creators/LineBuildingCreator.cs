namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="LineBuildingCreator" />.
    /// </summary>
    public class LineBuildingCreator : Creator
    {
        /// <summary>
        /// Defines the _buildingLinePrefab.
        /// </summary>
        [SerializeField] protected GameObject _buildingLinePrefab;

        /// <summary>
        /// Defines the idName.
        /// </summary>
        private const string idName = "buildings";

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public override void Create(dynamic collection, float scale, GameObject city)
        {
            var _parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Buildings);
            var buildingFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            if (buildingFeature == null)
            {
                return;
            }
            var polygons = CityGeneratorHelper.GetPolygonsForBuildings(buildingFeature);

            foreach (var poly in polygons)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_buildingLinePrefab, _parent);
                MultiLineStringHandler.UpdateLineRendererPoints(poly, out positions, scale, lineRenderer);
            }
        }
    }
}
