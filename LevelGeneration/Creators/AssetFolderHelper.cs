namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="AssetFolderHelper" />.
    /// </summary>
    public static class AssetFolderHelper
    {
        /// <summary>
        /// Defines the _assetFolder.
        /// </summary>
        private const string _assetFolder = "Assets";

        /// <summary>
        /// The CreateFolderToSaveCity.
        /// </summary>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        /// <param name="meshFolder">The meshFolder<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CreateFolderToSaveCity(GameObject city, string meshFolder)
        {
            var folderHierarchy = CreateFolderHierarchyFromString(meshFolder);

            folderHierarchy.Add(city.name);

            var oldFolderName = _assetFolder;
            string newFolderName;

            for (int i = 0; i < folderHierarchy.Count; i++)
            {
                newFolderName = folderHierarchy[i];
                var tempName = oldFolderName + "/" + newFolderName;

                if (!AssetDatabase.IsValidFolder(tempName))
                {
                    AssetDatabase.CreateFolder(oldFolderName, newFolderName);
                }

                oldFolderName += "/" + newFolderName;
            }

            return oldFolderName + "/";
        }

        /// <summary>
        /// The CreateFolderHierarchyFromString.
        /// </summary>
        /// <param name="meshFolder">The meshFolder<see cref="string"/>.</param>
        /// <returns>The <see cref="List{string}"/>.</returns>
        private static List<string> CreateFolderHierarchyFromString(string meshFolder)
        {
            var folderHierarchy = meshFolder.Split('/').ToList();

            if (folderHierarchy[0] == _assetFolder)
            {
                folderHierarchy.RemoveAt(0);
            }

            if (folderHierarchy.Last() == string.Empty)
            {
                folderHierarchy.RemoveAt(folderHierarchy.Count - 1);
            }

            return folderHierarchy;
        }
    }
}
