using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public class RoadCreator : Creator
    {
        [SerializeField] protected GameObject _roadPrefab;

        private const string idName = "roads";

        public override void Create(FeatureCollection collection, int scale, GameObject city)
        {
            var _roadParent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Rivers);
            var roadFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            var polygons = ((MultiLineString)roadFeature.Geometry).Coordinates;
            foreach (var poly in polygons)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_roadPrefab, _roadParent);
                MultiLineStringHandler.UpdateLineRendererPoints(poly, out positions, scale, lineRenderer);
            }
        }
    }
}