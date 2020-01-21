using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public class BuildingCreator : Creator
    {
        [SerializeField] private GameObject _buildingPrefab;

        private const string idName = "buildings";

        public override void Create(FeatureCollection collection, int scale, GameObject city)
        {
            var _parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Buildings);
            var buildingFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, idName);
            var polygons = ((MultiPolygon)buildingFeature.Geometry).Coordinates;
            int meshIndex = 0;

            foreach (var poly in polygons)
            {
                Mesh mesh = CityGeneratorHelper.CreateMeshFromGeoJsonPolygon(poly, _buildingPrefab, _parent, scale);
                string folderName = AssetFolderHelper.CreateFolderToSaveCity(city, MeshFolder.Folder);
                AssetDatabase.CreateAsset(mesh, folderName + meshIndex + ".asset");
                meshIndex++;
            }
        }


    }
}
