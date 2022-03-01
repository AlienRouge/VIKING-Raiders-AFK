using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float height;
        public Tile tile;
    }
    [SerializeField] public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    public void RenderMap(int width, int height, float[] noiseMap)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tile = GetTileUsingNoiseMap(noiseMap[y * width + x]);
                CreateTile(x, y, tile);
            }
        }
    }

    private void CreateTile(int x, int y, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    private Tile GetTileUsingNoiseMap(float noiseValue)
    {
        var tile = terrainLevel[terrainLevel.Count - 1].tile;
        foreach (var level in terrainLevel)
        {
            if (noiseValue < level.height)
            {
                tile = level.tile;
                break;
            }
        }

        return tile;
    }
}