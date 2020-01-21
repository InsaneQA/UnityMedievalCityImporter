using GeoJSON.Net.Feature;
using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    public abstract class Creator : MonoBehaviour
    {
        protected MeshFolder MeshFolder => GetComponent<MeshFolder>();

        public abstract void Create(FeatureCollection collection, int scale, GameObject city);
    }
}
