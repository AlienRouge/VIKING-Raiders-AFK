using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap notWalkableTilemap;

    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float height;
        public Tile tile;
        public bool walkable;
    }

    [SerializeField] public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    public void RenderMap(int width, int height, float[] noiseMap, int xOffset = 0, int yOffset = 0)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var terrain = GetTerrainLevel(noiseMap[y * width + x]);
                CreateTile(x, y, terrain, xOffset, yOffset);
            }
        }
    }

    private void CreateTile(int x, int y, TerrainLevel terrainLevel, int xOffset, int yOffset)
    {
        var tilemap = terrainLevel.walkable ? walkableTilemap : notWalkableTilemap;
        tilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), terrainLevel.tile);
    }
    
    private TerrainLevel GetTerrainLevel(float noiseValue)
    {
        var terrain = terrainLevel[terrainLevel.Count - 1];
        foreach (var level in terrainLevel)
        {
            if (noiseValue < level.height)
            {
                terrain = level;
                break;
            }
        }

        return terrain;
    }
}