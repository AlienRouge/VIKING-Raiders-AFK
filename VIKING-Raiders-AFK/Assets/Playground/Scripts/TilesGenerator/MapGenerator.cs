using System;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [Header("Map settings")] [SerializeField]
    private int width;

    [SerializeField] private int height;
    [SerializeField] private int spawnAreaWidth;

    [Header("Generation settings")] [SerializeField]
    private float scale;

    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;

    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;

    private int mapOffset;
    private int BattleAreaWidth => width - 2 * spawnAreaWidth;

    private NoiseMapRenderer _noiseMapRenderer;

    [SerializeField] private NavMeshSurface2d _navMeshSurface2d;

    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private Vector2 spawnAreaOffset;

    private void Start()
    {
        _noiseMapRenderer = GetComponent<NoiseMapRenderer>();
    }

    public void GenerateMap()
    {
        GenerateTilemap();
        BakeNavMesh();
    }

    public void GenerateTilemap()
    {
        GenerateSpawnAreas();
        GeneratePerlinMap();
    }

    public void BakeNavMesh()
    {
        _navMeshSurface2d.BuildNavMeshAsync();
    }

    public void SetSpawnAreas()
    {
        _spawnArea.transform.localScale = new Vector3(spawnAreaWidth - spawnAreaOffset.x, height - spawnAreaOffset.y);
        _spawnArea.transform.localPosition = new Vector3(spawnAreaWidth / 2f, height / 2f);
    }


    private void GenerateSpawnAreas()
    {
        var spawnMap = GenerateOneTileSpawnArea();
        _noiseMapRenderer.RenderMap(spawnAreaWidth, height, spawnMap);
        _noiseMapRenderer.RenderMap(spawnAreaWidth, height, spawnMap, width - spawnAreaWidth);
    }

    private float[] GenerateOneTileSpawnArea()
    {
        float[] map = new float[height * spawnAreaWidth];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < spawnAreaWidth; x++)
            {
                map[y * spawnAreaWidth + x] = 1.0f;
            }
        }

        return map;
    }

    private void GeneratePerlinMap()
    {
        float[] noiseMap =
            NoiseMapGenerator.GenerateNoiseMap(BattleAreaWidth, height, scale, octaves, persistence, lacunarity,
                offset);

        _noiseMapRenderer.RenderMap(BattleAreaWidth, height, noiseMap, spawnAreaWidth);
    }
}