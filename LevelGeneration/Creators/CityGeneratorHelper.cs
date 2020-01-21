using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public static class CityGeneratorHelper
    {
        public static Feature GetFeatureBasedOnPropertyValueName(FeatureCollection collection, string value)
        {
            return collection.Features.Where(coord => (string)coord.Properties["id"] == value).FirstOrDefault();
        }

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

        public static Vector3[] GetMeshVertices(Polygon poly, out Vector2 offset, int scale)
        {
            IList<Vector3> meshVertices = new List<Vector3>();

            var lineString = poly.Coordinates[0];
            offset = new Vector2(0.0f - (float)lineString.Coordinates[0].Latitude, 0.0f - (float)lineString.Coordinates[0].Longitude) * scale;
            foreach (var geoPosition in lineString.Coordinates)
            {
                var unityLatitude = (float)(scale * geoPosition.Latitude) + offset.x;
                var unityLongitude = (float)(scale * geoPosition.Longitude) + offset.y;
                meshVertices.Add(new Vector2(unityLatitude, unityLongitude));
            }

            return meshVertices.ToArray();
        }

        public static Mesh GetMeshAfterInstantiatingMeshObject(out GameObject meshObject, GameObject prefab, Transform parent)
        {
            var mesh = new Mesh();
            meshObject = Object.Instantiate(prefab, parent);
            meshObject.GetComponent<MeshFilter>().mesh = mesh;
            return mesh;
        }
        public static Mesh CreateMeshFromGeoJsonPolygon(Polygon poly, GameObject prefab, Transform parent, int scale)
        {
            GameObject meshObject;
            Mesh mesh = GetMeshAfterInstantiatingMeshObject(out meshObject, prefab, parent);
            Vector2 offset;
            var meshVertices = GetMeshVertices(poly, out offset, scale);
            var trianglesArray = GetTrianglesFromCoordinates(meshVertices);

            mesh.vertices = meshVertices;
            mesh.triangles = trianglesArray;
            mesh.RecalculateNormals();

            meshObject.transform.Translate(-offset);
            return mesh;
        }

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

        public static Transform GetParentTransformFromCity(GameObject city, CityElements parentType)
        {
            return city.transform.Find(parentType.ToString());
        }
    }
}
