namespace Assets.Scripts.Logic.LevelGeneration
{
    using Assets.Scripts.Logic.LevelGeneration.Creators;
    using Newtonsoft.Json;
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="CityGenerator" />.
    /// </summary>
    public class CityGenerator : MonoBehaviour
    {
        /// <summary>
        /// Defines the _jsonName.
        /// </summary>
        [Tooltip("The Json file name saved in the Resources folder")]
        [SerializeField] private string _jsonName;

        /// <summary>
        /// Defines the _cityPrefabPath.
        /// </summary>
        [Tooltip("The path to the created city prefab")]
        [SerializeField] private string _cityPrefabPath = "Assets/Prefabs/";

        /// <summary>
        /// Defines the _scale.
        /// </summary>
        [Tooltip("City scale")]
        [SerializeField] private float _scale = 0.01f;

        /// <summary>
        /// Defines the _updateCityTransformScale.
        /// </summary>
        [SerializeField] private bool _updateCityTransformScale = true;

        /// <summary>
        /// Defines the _transformScaleFactor.
        /// </summary>
        [SerializeField] private int _transformScaleFactor = 4;

        /// <summary>
        /// Defines the _creators.
        /// </summary>
        private Creator[] _creators;

        /// <summary>
        /// Defines the _json.
        /// </summary>
        private TextAsset _json;

        /// <summary>
        /// Defines the _fix.
        /// </summary>
        private JsonFix _fix;

        /// <summary>
        /// Defines the _city.
        /// </summary>
        public GameObject _city;

        /// <summary>
        /// The GenerateCity.
        /// </summary>
        public void GenerateCity()
        {
            ValidateFields();
            _city = CityGeneratorHelper.CreateCityInHierarchy(_jsonName);
            FixJson();
            dynamic collection = GetCollectionFromJson();
            LocateCreators();
            CreateCity(collection);
            UpdateCityTransformScale();
            SaveCity();
        }

        /// <summary>
        /// The UpdateCityTransformScale.
        /// </summary>
        private void UpdateCityTransformScale()
        {
            if (_updateCityTransformScale)
            {
                _city.transform.localScale *= _transformScaleFactor;
            }
        }

        /// <summary>
        /// The ValidateFields.
        /// </summary>
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

        /// <summary>
        /// The LocateCreators.
        /// </summary>
        private void LocateCreators()
        {
            _creators = GetComponentsInChildren<Creator>();
        }

        /// <summary>
        /// The FixJson.
        /// </summary>
        private void FixJson()
        {
            _json = Resources.Load<TextAsset>(_jsonName);
            var path = AssetDatabase.GetAssetPath(_json);
            _fix = GetComponent<JsonFix>();
            _fix.FixJson(_json, path);
        }

        /// <summary>
        /// The GetCollectionFromJson.
        /// </summary>
        /// <returns>The <see cref="FeatureCollection"/>.</returns>
        private dynamic GetCollectionFromJson()
        {
            _json = Resources.Load<TextAsset>(_jsonName);
            var collection = JsonConvert.DeserializeObject<dynamic>(_json.text);
            return collection;
        }

        /// <summary>
        /// The CreateCity.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        private void CreateCity(dynamic collection)
        {
            foreach (var creator in _creators)
            {
                if (creator.Enabled)
                {
                    creator.Create(collection, _scale, _city);
                }

            }
        }

        /// <summary>
        /// The SaveCity.
        /// </summary>
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
                Debug.LogWarning("The specified prefab " + _cityPrefabPath + " folder doesn't exist. Please prefabricate the city manually or create the folder and re-run city generation");
            }
        }
    }
}
