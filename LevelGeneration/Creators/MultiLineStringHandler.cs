using GeoJSON.Net.Geometry;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public static class MultiLineStringHandler
    {
        public static LineRenderer CreateLineRenderer(GameObject prefab, Transform parent)
        {
            var lineRendererObject = Object.Instantiate(prefab, parent);
            return lineRendererObject.GetComponent<LineRenderer>();
        }

        public static void UpdateLineRendererPoints(LineString poly, out Vector3[] positions, int scale, LineRenderer lineRenderer)
        {
            positions = GetPositionsFromLineString(poly, scale);
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }

        public static Vector3[] GetPositionsFromLineString(LineString poly, int scale)
        {
            var coordinates = poly.Coordinates.Select(coordinate => new Vector3((float)coordinate.Latitude, (float)coordinate.Longitude, 0) * scale);
            return coordinates.ToArray();
        }
    }
}