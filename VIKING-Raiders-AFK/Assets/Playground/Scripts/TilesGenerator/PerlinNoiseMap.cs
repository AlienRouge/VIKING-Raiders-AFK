using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class PerlinNoiseMap : MonoBehaviour
{
    // [SerializeField] private Tilemap tilemap;
    // [SerializeField] private List<Tile> tiles;
    //
    // private Dictionary<float, Tile> tileSet;
    // [SerializeField] private List<float> values;
    // private Dictionary<float, GameObject> tileGroups;
    //
    // [SerializeField] private int mapWidth, mapHeight;
    //
    // [SerializeField] private List<List<int>> noiseGrid = new List<List<int>>();
    // [SerializeField] private List<List<Tile>> tileGrid = new List<List<Tile>>();
    //
    // private float magnification = 7.0f;
    // private float xOffset = 0;
    // private float yOffset = 0;
    //
    //
    // private void Start()
    // {
    //     CreateTileSet();
    //     CreateTileGroups();
    //     GenerateMap();
    // }
    //
    // private void CreateTileSet()
    // {
    //     tileSet = new Dictionary<float, Tile>
    //     {
    //         { values[0], tiles[0] },
    //         { values[1], tiles[1] }
    //     };
    // }
    //
    // private void CreateTileGroups()
    // {
    //     tileGroups = new Dictionary<float, GameObject>();
    //     foreach (var prefabPair in tileSet)
    //     {
    //         GameObject tileGroup = new GameObject(prefabPair.Value.ToString());
    //         Debug.Log(prefabPair.Value.ToString());
    //         tileGroup.transform.parent = gameObject.transform;
    //         tileGroup.transform.localPosition = new Vector3(0, 0, 0);
    //         tileGroups.Add(prefabPair.Key, tileGroup);
    //     }
    // }
    //
    // private void GenerateMap()
    // {
    //     for (int x = 0; x < mapWidth; x++)
    //     {
    //         noiseGrid.Add(new List<int>());
    //         tileGrid.Add(new List<Tile>());
    //         
    //         for (int y = 0; y < mapHeight; y++)
    //         {
    //             int tileId = GetTileIdUsingPerlin(x, y);
    //             noiseGrid[x].Add(tileId);
    //             CreateTile(tileId, x, y);
    //         }
    //     }
    // }
    //
    // private int GetTileIdUsingPerlin(int x, int y)
    // {
    //     float rawPerlin =  Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
    //     float clampPerlin = Mathf.Clamp01(rawPerlin);
    //
    //     if (clampPerlin <=0.3)
    //     {
    //         return 0;
    //     }
    //     else if (clampPerlin>0.3)
    //     {
    //         return 1;
    //     }
    //
    //     return -1;
    // }
    //
    // private void CreateTile(int id, int x, int y)
    // {
    //     tilemap.SetTile(new Vector3Int(x, y, 0), tiles[id]);
    // }
    
}