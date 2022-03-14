using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseMapText))]
public class MapGeneratorEditorText : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseMapText mapGen = (NoiseMapText)target;
        
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