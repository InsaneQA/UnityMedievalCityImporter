namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="MultiLineStringHandler" />.
    /// </summary>
    public static class MultiLineStringHandler
    {
        /// <summary>
        /// The CreateLineRenderer.
        /// </summary>
        /// <param name="prefab">The prefab<see cref="GameObject"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <returns>The <see cref="LineRenderer"/>.</returns>
        public static LineRenderer CreateLineRenderer(GameObject prefab, Transform parent)
        {
            var lineRendererObject = Object.Instantiate(prefab, parent);
            return lineRendererObject.GetComponent<LineRenderer>();
        }

        /// <summary>
        /// The UpdateLineRendererPoints.
        /// </summary>
        /// <param name="poly">The poly<see cref="LineString"/>.</param>
        /// <param name="positions">The positions<see cref="Vector3[]"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="lineRenderer">The lineRenderer<see cref="LineRenderer"/>.</param>
        //public static void UpdateLineRendererPoints(LineString poly, out Vector3[] positions, int scale, LineRenderer lineRenderer)
        //{
        //    positions = GetPositionsFromLineString(poly, scale);
        //    lineRenderer.positionCount = positions.Length;
        //    lineRenderer.SetPositions(positions);
        //}

        /// <summary>
        /// The UpdateLineRendererPoints.
        /// </summary>
        /// <param name="poly">The poly<see cref="Polygon"/>.</param>
        /// <param name="positions">The positions<see cref="Vector3[]"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="lineRenderer">The lineRenderer<see cref="LineRenderer"/>.</param>
        public static void UpdateLineRendererPoints(dynamic poly, out Vector3[] positions, float scale, LineRenderer lineRenderer)
        {
            Vector2 offset;
            positions = GetPositionsFromPolygon(poly, scale, out offset);
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
            lineRenderer.transform.position -= new Vector3(offset.x, offset.y, 0);
        }

        /// <summary>
        /// The GetPositionsFromLineString.
        /// </summary>
        /// <param name="poly">The poly<see cref="LineString"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <returns>The <see cref="Vector3[]"/>.</returns>
        public static Vector3[] GetPositionsFromLineString(dynamic poly, float scale)
        {
            var coordinates = GetCoordinatesFromPoly(poly);
            List<Vector3> vectorCoordinates = new List<Vector3>();

            foreach (var coordinate in coordinates)
            {
                float x = coordinate[0];
                float y = coordinate[1];
                vectorCoordinates.Add(new Vector3(x, y, 0) * scale);
            }
            //coordinate => new Vector3((float)coordinate.Latitude, (float)coordinate.Longitude, 0) * scale);
            return vectorCoordinates.ToArray();
        }

        /// <summary>
        /// The GetPositionsFromPolygon.
        /// </summary>
        /// <param name="poly">The poly<see cref="Polygon"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="offset">The offset<see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector3[]"/>.</returns>
        public static Vector3[] GetPositionsFromPolygon(dynamic poly, float scale, out Vector2 offset)
        {
            return CityGeneratorHelper.GetMeshVertices(poly, out offset, scale);
        }

        public static dynamic GetCoordinatesFromPoly(dynamic poly)
        {
            dynamic coordinates = poly;
            try
            {
                coordinates = poly["coordinates"];
            }
            catch
            {
            }

            var arrayIsFound = false;
            dynamic currentArray = coordinates;
            while (!arrayIsFound)
            {
                if (currentArray.Count > 1)
                {
                    arrayIsFound = true;
                }
                else
                {
                    try
                    {
                        currentArray = currentArray[0];
                    }
                    catch
                    {
                        return currentArray;
                    }
                }
            }
            return currentArray;
        }
    }
}
