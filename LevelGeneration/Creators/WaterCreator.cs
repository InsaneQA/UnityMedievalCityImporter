using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public class WaterCreator : Creator
    {
        [SerializeField] private GameObject _waterPrefab;
        private string _idName = "water";

        public override void Create(FeatureCollection collection, int scale, GameObject city)
        {
            var parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Water);
            var buildingFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, _idName);
            var polygon = (Polygon)buildingFeature.Geometry;

            Mesh mesh = CityGeneratorHelper.CreateMeshFromGeoJsonPolygon(polygon, _waterPrefab, parent, scale);
            string folderName = AssetFolderHelper.CreateFolderToSaveCity(city, MeshFolder.Folder);
            AssetDatabase.CreateAsset(mesh, folderName + "water.asset");
        }
    }
}
