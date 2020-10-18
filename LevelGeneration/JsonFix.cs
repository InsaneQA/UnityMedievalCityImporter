namespace Assets.Scripts.Logic.LevelGeneration
{
    using Assets.Scripts.Logic.LevelGeneration.Creators;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="JsonFix" />.
    /// </summary>
    public class JsonFix : MonoBehaviour
    {
        /// <summary>
        /// The FixJson.
        /// </summary>
        /// <param name="jsonAsset">The jsonAsset<see cref="TextAsset"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public void FixJson(TextAsset jsonAsset, string path)
        {
            JObject rss = JObject.Parse(jsonAsset.text);
            Resources.UnloadAsset(jsonAsset);
            JObject buildingsFeature = (JObject)CityGeneratorHelper.GetFeatureBasedOnPropertyValueName(rss, "buildings");
            JArray coordinates = (JArray)buildingsFeature["coordinates"];
            FixCoordinates(coordinates);
            SaveToFile(rss, path);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// The FixCoordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates<see cref="JArray"/>.</param>
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

        /// <summary>
        /// The MarkPolygonToBeRemovedIfThereAreLessThanFourCoordinates.
        /// </summary>
        /// <param name="toBeRemoved">The toBeRemoved<see cref="IList{JToken}"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="JToken"/>.</param>
        /// <param name="meshCoordinates">The meshCoordinates<see cref="JArray"/>.</param>
        private static void MarkPolygonToBeRemovedIfThereAreLessThanFourCoordinates(IList<JToken> toBeRemoved, JToken coordinate, JArray meshCoordinates)
        {
            if (meshCoordinates.Count <= 3)
            {
                toBeRemoved.Add(coordinate);
            }
        }

        /// <summary>
        /// The AddCoordinatesIfTheEndIsMissing.
        /// </summary>
        /// <param name="meshCoordinates">The meshCoordinates<see cref="JArray"/>.</param>
        private static void AddCoordinatesIfTheEndIsMissing(JArray meshCoordinates)
        {
            if (meshCoordinates[0].ToString() != meshCoordinates[meshCoordinates.Count - 1].ToString())
            {
                meshCoordinates.Add(meshCoordinates[0]);
            }
        }

        /// <summary>
        /// The SaveToFile.
        /// </summary>
        /// <param name="rss">The rss<see cref="JObject"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        private void SaveToFile(JObject rss, string path)
        {
            File.Delete(path);
            File.WriteAllText(path, rss.ToString());
        }
    }
}
