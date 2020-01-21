using UnityEngine;

namespace Assets.Scripts.Logic.LevelGeneration
{
    public class MeshFolder : MonoBehaviour
    {
        [Tooltip("The folder in which the meshes are going to be saved. The city name will be added to the end of the line")]
        [SerializeField] private string _folder = "Assets/Levels/Meshes/";
        public string Folder => _folder;
    }
}
