using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseMap))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseMap mapGen = (NoiseMap)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
            DrawDefaultInspector();
        }
        
        
        
        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}