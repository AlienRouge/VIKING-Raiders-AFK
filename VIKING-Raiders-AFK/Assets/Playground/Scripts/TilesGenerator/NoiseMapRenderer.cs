using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Tile> tiles;
    [SerializeField] private List<float> values;

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
                var tileID = GetTileIdUsingNoiseMap(noiseMap[y * width + x]);
                CreateTile(x, y, tileID);
            }
        }
    }

    private void CreateTile(int x, int y, int id)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tiles[id]);
    }

    private int GetTileIdUsingNoiseMap(float noiseValue)
    {
        switch (noiseValue)
        {
            case var n when (n >= 0 && n < values[0]):
                return 0;
            case var n when (n >= values[0] && n < values[1]):
                return 1;
            case var n when (n >= values[1] && n <= values[2]):
                return 2;
            default:
                Debug.Log(noiseValue);
                return -1;
        }
    }
}