namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="CityGeneratorHelper" />.
    /// </summary>
    public static class CityGeneratorHelper
    {
        /// <summary>
        /// The GetFeatureBasedOnPropertyValueName.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="Feature"/>.</returns>
        public static dynamic GetFeatureBasedOnPropertyValueName(dynamic collection, string value)
        {
            var features = collection["features"];
            foreach (var feature in features)
            {
                if (feature["id"] == value)
                {
                    return feature;
                }
            }
            return null;
        }

        internal static dynamic GetPolygonsFromFeature(dynamic feature)
        {
            var geometries = feature["geometries"];

            if (geometries != null)
            {
                return geometries;
            }

            return feature;

            //Dictionary<dynamic, dynamic> dict = new Dictionary<dynamic, dynamic>();
            //foreach (var element in feature)
            //{
            //    if (element.Name == "coordinates")
            //    {
            //        dict[element.Name] = element.Value;
            //    }
            //}

            //return dict;
        }

        internal static dynamic GetPolygonsForBuildings(dynamic feature)
        {
            var listOfPolygons = new List<dynamic>();
            var coordinates = feature["coordinates"];
            foreach (var element in coordinates)
            {
                listOfPolygons.Add(element[0]);
            }

            return listOfPolygons;
        }

        /// <summary>
        /// The GetTrianglesFromCoordinates.
        /// </summary>
        /// <param name="meshVertices">The meshVertices<see cref="IList{Vector3}"/>.</param>
        /// <returns>The <see cref="int[]"/>.</returns>
        public static int[] GetTrianglesFromCoordinates(IList<Vector3> meshVertices)
        {
            IList<int> meshTriangles = new List<int>();
            var counter = 2;

            while (counter != meshVertices.Count)
            {
                meshTriangles.Add(0);
                meshTriangles.Add(counter);
                meshTriangles.Add(counter - 1);

                counter++;
            }

            return meshTriangles.ToArray();
        }

        /// <summary>
        /// The GetMeshVertices.
        /// </summary>
        /// <param name="poly">The poly<see cref="Polygon"/>.</param>
        /// <param name="offset">The offset<see cref="Vector2"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <returns>The <see cref="Vector3[]"/>.</returns>
        public static Vector3[] GetMeshVertices(dynamic poly, out Vector2 offset, float scale)
        {
            IList<Vector3> meshVertices = new List<Vector3>();
            var lineString = MultiLineStringHandler.GetCoordinatesFromPoly(poly);
            offset = new Vector2(0.0f - float.Parse(lineString[0][0].ToString()), 0.0f - float.Parse(lineString[0][1].ToString())) * scale;
            foreach (var geoPosition in lineString)
            {
                var unityLatitude = (float)(scale * float.Parse(geoPosition[0].ToString())) + offset.x;
                var unityLongitude = (float)(scale * float.Parse(geoPosition[1].ToString())) + offset.y;
                meshVertices.Add(new Vector2(unityLatitude, unityLongitude));
            }

            return meshVertices.Reverse().ToArray();
        }

        /// <summary>
        /// The GetMeshAfterInstantiatingMeshObject.
        /// </summary>
        /// <param name="meshObject">The meshObject<see cref="GameObject"/>.</param>
        /// <param name="prefab">The prefab<see cref="GameObject"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <returns>The <see cref="Mesh"/>.</returns>
        public static Mesh GetMeshAfterInstantiatingMeshObject(out GameObject meshObject, GameObject prefab, Transform parent)
        {
            var mesh = new Mesh();
            meshObject = Object.Instantiate(prefab, parent);
            meshObject.GetComponent<MeshFilter>().mesh = mesh;
            return mesh;
        }

        /// <summary>
        /// The CreateMeshFromGeoJsonPolygon.
        /// </summary>
        /// <param name="poly">The poly<see cref="Polygon"/>.</param>
        /// <param name="prefab">The prefab<see cref="GameObject"/>.</param>
        /// <param name="parent">The parent<see cref="Transform"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="material">The material<see cref="Material"/>.</param>
        /// <returns>The <see cref="Mesh"/>.</returns>
        public static Mesh CreateMeshFromGeoJsonPolygon(dynamic poly, GameObject prefab, Transform parent, float scale, Material material = null)
        {
            GameObject meshObject;
            Mesh mesh = GetMeshAfterInstantiatingMeshObject(out meshObject, prefab, parent);
            Vector2 offset;
            var meshVertices = GetMeshVertices(poly, out offset, scale);
            var uvs = GetMeshUVsFromVertices(meshVertices);
            var trianglesArray = GetTrianglesFromCoordinates(meshVertices);

            mesh.vertices = meshVertices;
            mesh.triangles = trianglesArray;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            meshObject.transform.Translate(-offset);

            if (material != null)
            {
                meshObject.GetComponent<MeshRenderer>().material = material;
            }

            return mesh;
        }

        /// <summary>
        /// The GetMeshUVsFromVertices.
        /// </summary>
        /// <param name="meshVertices">The meshVertices<see cref="Vector3[]"/>.</param>
        /// <returns>The <see cref="Vector2[]"/>.</returns>
        private static Vector2[] GetMeshUVsFromVertices(Vector3[] meshVertices)
        {
            var uvs = new Vector2[meshVertices.Length];

            uvs[0] = Vector2.zero;
            uvs[1] = Vector2.right;
            uvs[2] = new Vector2(1, 1);
            uvs[3] = Vector2.up;

            return uvs;
        }

        /// <summary>
        /// The CreateCityInHierarchy.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="GameObject"/>.</returns>
        public static GameObject CreateCityInHierarchy(string name)
        {
            var city = new GameObject(name);
            new GameObject(CityElements.Buildings.ToString()).transform.parent = city.transform;
            new GameObject(CityElements.Walls.ToString()).transform.parent = city.transform;
            new GameObject(CityElements.Roads.ToString()).transform.parent = city.transform;
            new GameObject(CityElements.Water.ToString()).transform.parent = city.transform;
            new GameObject(CityElements.Rivers.ToString()).transform.parent = city.transform;
            return city;
        }

        /// <summary>
        /// The GetParentTransformFromCity.
        /// </summary>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        /// <param name="parentType">The parentType<see cref="CityElements"/>.</param>
        /// <returns>The <see cref="Transform"/>.</returns>
        public static Transform GetParentTransformFromCity(GameObject city, CityElements parentType)
        {
            return city.transform.Find(parentType.ToString());
        }
    }
}
