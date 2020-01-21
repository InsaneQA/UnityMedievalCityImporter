using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public class WallCreator : Creator
    {
        [SerializeField] private bool _createWallsSeparately = true;
        [SerializeField] private GameObject _towerPrefab;
        [SerializeField] private GameObject _wallPrefab;

        private const string _idName = "walls";

        public override void Create(FeatureCollection collection, int scale, GameObject city)
        {
            var parent = CityGeneratorHelper.GetParentTransformFromCity(city, CityElements.Walls);
            var wallFeature = CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(collection, _idName);
            var lineStrings = ((MultiLineString)wallFeature.Geometry).Coordinates;

            if (_createWallsSeparately)
            {
                CreateWallsSeparately(scale, parent, lineStrings);
            }
            else
            {
                CreateContiniousWall(scale, parent, lineStrings);
            }
        }

        private void CreateContiniousWall(int scale, Transform parent, ReadOnlyCollection<LineString> lineStrings)
        {
            foreach (var lines in lineStrings)
            {
                Vector3[] positions;
                var lineRenderer = MultiLineStringHandler.CreateLineRenderer(_wallPrefab, parent);
                MultiLineStringHandler.UpdateLineRendererPoints(lines, out positions, scale, lineRenderer);

                buildTowers(parent, positions);
            }
        }

        private void buildTowers(Transform parent, Vector3[] positions)
        {
            foreach (var position in positions)
            {
                CreateTower(position, parent);
            }
        }

        private void CreateWallsSeparately(int scale, Transform parent, ReadOnlyCollection<LineString> lineStrings)
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

                buildTowers(parent, positions);
            }
        }

        private void CreateTower(Vector3 position, Transform parent)
        {
            Instantiate(_towerPrefab, position, Quaternion.Euler(90f, 0f, 0f), parent);
        }
    }
}