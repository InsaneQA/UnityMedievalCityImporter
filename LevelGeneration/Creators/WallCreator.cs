namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using System.Collections.ObjectModel;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="WallCreator" />.
    /// </summary>
    public class WallCreator : Creator
    {
        /// <summary>
        /// Defines the _createWallsSeparately.
        /// </summary>
        [SerializeField] private bool _createWallsSeparately = true;

        /// <summary>
        /// Defines the _towerPrefab.
        /// </summary>
        [SerializeField] private GameObject _towerPrefab;

        /// <summary>
        /// Defines the _wallPrefab.
        /// </summary>
        [SerializeField] private GameObject _wallPrefab;

        /// <summary>
        /// Defines the _idName.
        /// </summary>
        private const string _idName = "walls";

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public override void Create(dynamic collection, float scale, GameObject city)
        {
            var parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Walls);
            var wallFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, _idName);
            if (wallFeature == null)
            {
                return;
            }
            var lineStrings = CityGeneratorHelper.GetPolygonsFromFeature(wallFeature);

            if (_createWallsSeparately)
            {
                CreateWallsSeparately(scale, parent, lineStrings);
            }
            else
            {
                CreateContiniousWall(scale, parent, lineStrings);
            }
        }

        /// <summary>
        /// The CreateContiniousWall.
        /// </summary>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <param name="lineStrings">The lineStrings<see cref="ReadOnlyCollection{LineString}"/>.</param>
        private void CreateContiniousWall(float scale, Transform parent, dynamic lineStrings)
        {
            foreach (var lines in lineStrings)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_wallPrefab, parent);
                MultiLineStringHandler.UpdateLineRendererPoints(lines, out positions, scale, lineRenderer);

                buildTowers(parent, positions, scale);
            }
        }

        /// <summary>
        /// The buildTowers.
        /// </summary>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <param name="positions">The positions<see cref="Vector3[]"/>.</param>
        private void buildTowers(Transform parent, Vector3[] positions, float scale)
        {
            foreach (var position in positions)
            {
                CreateTower(position, parent, scale);
            }
        }

        /// <summary>
        /// The CreateWallsSeparately.
        /// </summary>
        /// <param name="scale">The scale<see cref="float"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <param name="lineStrings">The lineStrings<see cref="ReadOnlyCollection{LineString}"/>.</param>
        private void CreateWallsSeparately(float scale, Transform parent, dynamic lineStrings)
        {
            foreach (var lines in lineStrings)
            {
                Vector3[] positions = MultiLineStringHandler.GetPositionsFromLineString(lines, scale);

                for (int i = 0; i < positions.Length - 1; i++)
                {
                    var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_wallPrefab, parent);
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, positions[i]);
                    lineRenderer.SetPosition(1, positions[i + 1]);
                }

                buildTowers(parent, positions, scale);
            }
        }

        /// <summary>
        /// The CreateTower.
        /// </summary>
        /// <param name="position">The position<see cref="Vector3"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        private void CreateTower(Vector3 position, Transform parent, float scale)
        {
            var tower = Instantiate(_towerPrefab, position, Quaternion.Euler(90f, 0f, 0f), parent);
            tower.transform.localScale = tower.transform.localScale * (scale / 0.03f);
        }
    }
}
