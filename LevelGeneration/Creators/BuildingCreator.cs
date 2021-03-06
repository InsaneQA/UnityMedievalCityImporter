﻿namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="BuildingCreator" />.
    /// </summary>
    public class BuildingCreator : Creator
    {
        /// <summary>
        /// Defines the _buildingPrefab.
        /// </summary>
        [SerializeField] private GameObject _buildingPrefab;

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

            int meshIndex = 0;

            string folderName = AssetFolderHelper.CreateFolderToSaveCity(city, MeshFolder.Folder);


            foreach (var poly in polygons)
            {
                Mesh mesh = CityGeneratorHelper.CreateMeshFromGeoJsonPolygon(poly, _buildingPrefab, _parent, scale);
                AssetDatabase.CreateAsset(mesh, folderName + meshIndex + ".asset");
                meshIndex++;
            }
        }
    }
}
