using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.Logic.LevelGeneration
{
    public class JsonFix : MonoBehaviour
    {
        public void FixJson(TextAsset jsonAsset, string path)
        {
            JObject rss = JObject.Parse(jsonAsset.text);
            Resources.UnloadAsset(jsonAsset);
            JArray features = (JArray)rss["features"];
            JObject buildingsFeature = (JObject)features[4];
            JArray coordinates = (JArray)buildingsFeature["geometry"]["coordinates"];
            FixCoordinates(coordinates);
            SaveToFile(rss, path);
            AssetDatabase.Refresh();
        }

        private static void FixCoordinates(JArray coordinates)
        {
            IList<JToken> toBeRemoved = new List<JToken>();

            foreach (var coordinate in coordinates)
            {
                JArray meshCoordinates = (JArray)coordinate[0];
                AddCoordinatesIfTheEndIsMissing(meshCoordinates);
                MarkPolygonToBeRemovedIfThereAreLessThanFourCoordinates(toBeRemoved, coordinate, meshCoordinates);
            }

            foreach (var coordinate in toBeRemoved)
            {
                coordinates.Remove(coordinate);
            }
        }

        private static void MarkPolygonToBeRemovedIfThereAreLessThanFourCoordinates(IList<JToken> toBeRemoved, JToken coordinate, JArray meshCoordinates)
        {
            if (meshCoordinates.Count <= 3)
            {
                toBeRemoved.Add(coordinate);
            }
        }

        private static void AddCoordinatesIfTheEndIsMissing(JArray meshCoordinates)
        {
            if (meshCoordinates[0].ToString() != meshCoordinates[meshCoordinates.Count - 1].ToString())
            {
                meshCoordinates.Add(meshCoordinates[0]);
            }
        }

        private void SaveToFile(JObject rss, string path)
        {
            File.Delete(path);
            File.WriteAllText(path, rss.ToString());
        }
    }
}