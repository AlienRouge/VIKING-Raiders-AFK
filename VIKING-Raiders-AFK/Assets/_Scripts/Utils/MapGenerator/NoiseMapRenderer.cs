using System;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    private Tilemap _walkable;
    private Tilemap _notWalkable;
    private Tilemap _decor;

    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float height;
        public Tile tile;
        public TerrainLevelType levelType;
    }

    [SerializeField] public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    public void Init(Tilemap walkable, Tilemap notWalkable, Tilemap decor)
    {
        _walkable = walkable;
        _notWalkable = notWalkable;
        _decor = decor;
    }

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
        
        var tilemap = terrainLevel.levelType switch
        {
            TerrainLevelType.Walkable => _walkable,
            TerrainLevelType.NotWalkable => _notWalkable,
            TerrainLevelType.Decor => _decor,
            _ => _walkable
        };
        
        tilemap.SetTile(new Vector3Int(x + xOffset, y + yOffset, 0), terrainLevel.tile);
    }

    public void ClearTilemaps()
    {
        _walkable.ClearAllTiles();
        _walkable.CompressBounds();
        _notWalkable.ClearAllTiles();
        _notWalkable.CompressBounds();
    }

    public List<TerrainLevel> GetTerrainLevels()
    {
        return terrainLevel;
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