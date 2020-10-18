namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="WaterCreator" />.
    /// </summary>
    public class WaterCreator : Creator
    {
        /// <summary>
        /// Defines the _waterPrefab.
        /// </summary>
        [SerializeField] private GameObject _waterPrefab;

        /// <summary>
        /// Defines the _idName.
        /// </summary>
        private string _idName = "water";

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public override void Create(dynamic collection, float scale, GameObject city)
        {
            var parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Water);
            var buildingFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, _idName);
            if (buildingFeature == null)
            {
                return;
            }
            var polygon = CityGeneratorHelper.GetPolygonsFromFeature(buildingFeature);

            Mesh mesh = CityGeneratorHelper.CreateMeshFromGeoJsonPolygon(polygon, _waterPrefab, parent, scale);
            string folderName = AssetFolderHelper.CreateFolderToSaveCity(city, MeshFolder.Folder);
            AssetDatabase.CreateAsset(mesh, folderName + "water.asset");
        }
    }
}
