using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Tilemap))]
public class TilemapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tilemap = target as Tilemap;

        if (tilemap.GetUsedTilesCount() > 0 && GUILayout.Button("Clear") && EditorUtility.DisplayDialog("Clear Tilemap", "Are you sure you want to clear all tiles?", "Ok", "Cancel"))
        {
            tilemap.ClearAllTiles();
            tilemap.CompressBounds();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Compress Bounds"))
        {
            var bounds = tilemap.cellBounds;
            tilemap.CompressBounds();
            if (tilemap.cellBounds != bounds) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}