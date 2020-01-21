using Assets.Scripts.Logic.LevelGeneration.Creators;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.Logic.LevelGeneration
{
    public class CityGenerator : MonoBehaviour
    {
        [Tooltip("The Json file name saved in the Resources folder")]
        [SerializeField] private string _jsonName;

        [Tooltip("The path to the created city prefab")]
        [SerializeField] private string _cityPrefabPath = "Assets/Prefabs/";

        [Tooltip("City scale")]
        [SerializeField] private int _scale = 2000;
        private Creator[] _creators;
        private TextAsset _json;
        private JsonFix _fix;
        GameObject _city;

        public void GenerateCity()
        {
            ValidateFields();
            _city = CityGeneratorHelper.CreateCityInHierarchy(_jsonName);
            FixJson();
            FeatureCollection collection = GetCollectionFromJson();
            LocateCreators();
            CreateCity(collection);
            SaveCity();
        }

        private void ValidateFields()
        {
            if (_jsonName == string.Empty)
            {
                throw new ArgumentException("Please specify the Json Name");
            }

            if (_cityPrefabPath == string.Empty)
            {
                throw new ArgumentException("Please specify the City Prefab Path, for example \"Assets/Prefabs/\"");
            }
        }

        private void LocateCreators()
        {
            _creators = GetComponentsInChildren<Creator>();
        }

        private void FixJson()
        {
            _json = Resources.Load<TextAsset>(_jsonName);
            var path = AssetDatabase.GetAssetPath(_json);
            _fix = GetComponent<JsonFix>();
            _fix.FixJson(_json, path);
        }



        private FeatureCollection GetCollectionFromJson()
        {
            _json = Resources.Load<TextAsset>(_jsonName);
            var collection = JsonConvert.DeserializeObject<FeatureCollection>(_json.text);
            return collection;
        }

        private void CreateCity(FeatureCollection collection)
        {
            foreach (var creator in _creators)
            {
                creator.Create(collection, _scale, _city);
            }
        }

        private void SaveCity()
        {
            bool isSaved;
            try
            {
                PrefabUtility.SaveAsPrefabAsset(_city, _cityPrefabPath + _jsonName + ".prefab", out isSaved);
                if (isSaved)
                {
                    Debug.Log("The city has been prefabricated!");
                }
            }
            catch (ArgumentException)
            {
                Debug.LogError("The specified prefab " + _cityPrefabPath + " folder doesn't exist. Please prefabricate the city manually or create the folder and re-run city generation");
            }
        }
    }
}