using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public static class AssetFolderHelper
    {
        private const string _assetFolder = "Assets";

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
