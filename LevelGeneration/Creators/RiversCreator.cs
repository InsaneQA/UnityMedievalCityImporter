using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public class RiversCreator : Creator
    {
        [SerializeField] protected GameObject _riverPrefab;

        private const string idName = "rivers";

        public override void Create(FeatureCollection collection, int scale, GameObject city)
        {
            var _parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Rivers);
            var roadFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            var polygons = ((MultiLineString)roadFeature.Geometry).Coordinates;
            foreach (var poly in polygons)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_riverPrefab, _parent);
                MultiLineStringHandler.UpdateLineRendererPoints(poly, out positions, scale, lineRenderer);
            }
        }
    }
}