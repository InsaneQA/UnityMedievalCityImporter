namespace Assets.Scripts.Logic.LevelGeneration.Creators
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Creator" />.
    /// </summary>
    public abstract class Creator : MonoBehaviour
    {
        /// <summary>
        /// Defines the _enabled.
        /// </summary>
        [SerializeField] protected bool _enabled = true;

        /// <summary>
        /// Gets a value indicating whether Enabled.
        /// </summary>
        public bool Enabled => _enabled;

        /// <summary>
        /// Gets the MeshFolder.
        /// </summary>
        protected MeshFolder MeshFolder => GetComponent<MeshFolder>();

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="collection">The collection<see cref="FeatureCollection"/>.</param>
        /// <param name="scale">The scale<see cref="int"/>.</param>
        /// <param name="city">The city<see cref="GameObject"/>.</param>
        public abstract void Create(dynamic collection, float scale, GameObject city);
    }
}
