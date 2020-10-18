namespace Assets.Scripts.Logic.LevelGeneration
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="MeshFolder" />.
    /// </summary>
    public class MeshFolder : MonoBehaviour
    {
        /// <summary>
        /// Defines the _folder.
        /// </summary>
        [Tooltip("The folder in which the meshes are going to be saved. The city name will be added to the end of the line")]
        [SerializeField] private string _folder = "Assets/Levels/Meshes/";

        /// <summary>
        /// Gets the Folder.
        /// </summary>
        public string Folder => _folder;
    }
}
