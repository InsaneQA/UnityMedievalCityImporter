using Assets.Scripts.Logic.LevelGeneration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CityGenerator cityGenerator = (CityGenerator)target;
        if (GUILayout.Button("Generate City Prefab"))
        {
            cityGenerator.GenerateCity();
        }
    }
}
