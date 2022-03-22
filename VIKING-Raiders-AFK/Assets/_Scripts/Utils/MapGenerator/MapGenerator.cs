using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    private static MapGenerator _instance;

    [Header("Map settings")] [SerializeField]
    private MapController _mapPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int spawnAreaWidth;
    [SerializeField] private Vector2 spawnAreaOffset;

    [Header("Generation settings")] [SerializeField]
    private float scale;

    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;
    [SerializeField] private Vector2 offset;

    private MapController _mapController;
    private int BattleAreaWidth => width - 2 * spawnAreaWidth;
    private NoiseMapRenderer _noiseMapRenderer;

    public static MapGenerator instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(MapGenerator) + " is NULL!");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _noiseMapRenderer = GetComponent<NoiseMapRenderer>();
    }

    public void TryInstantiateMap()
    {
        _mapController = FindObjectOfType<MapController>();
        if (_mapController == null)
        {
            _mapController = Instantiate(_mapPrefab);
            // // world height is always camera's orthographicSize * 2
            // float worldScreenHeight = Camera.main.orthographicSize * 2;
            //
            // // world width is calculated by diving world height with screen heigh
            // // then multiplying it with screen width
            // float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
            //
            // // to scale the game object we divide the world screen width with the
            // // size x of the sprite, and we divide the world screen height with the
            // // size y of the sprite
            //
            // Debug.Log(worldScreenWidth / width);
            // Debug.Log(worldScreenWidth / height);
            //
            // var newXScale = worldScreenWidth / width - (worldScreenWidth / width) * 0.25f;
            // var newYScale = worldScreenHeight / height - (worldScreenWidth / width) * 0.1f;
            //
            // _mapController.transform.localScale = new Vector3(newXScale, newYScale, 1);
            _mapController.transform.localPosition = new Vector3(
                -width/2f,
                -height/2f, 1);
        }

        _noiseMapRenderer.Init(_mapController.walkableTilemap, _mapController.notWalkableTilemap,
            _mapController.decorTilemap);
    }

    public MapController GenerateMap()
    {
        GenerateTilemap();
        _mapController.BakeMap();
        SetupSpawnAreas();

        return _mapController;
    }

    public void GenerateTilemap()
    {
        TryInstantiateMap();
        _noiseMapRenderer.ClearTilemaps();
        GenerateSpawnAreas();
        GenerateBattleArea();
    }

    public void SetupSpawnAreas()
    {
        var spawnAreaScale = new Vector3(spawnAreaWidth - spawnAreaOffset.x, height - spawnAreaOffset.y);

        _mapController.spawnAreaController.SetupSpawnArea(Team.Team1, spawnAreaScale,
            new Vector3(spawnAreaWidth / 2f, height / 2f));

        _mapController.spawnAreaController.SetupSpawnArea(Team.Team2, spawnAreaScale,
            new Vector3(width - spawnAreaWidth / 2f, height / 2f));
    }

    private void GenerateSpawnAreas()
    {
        _noiseMapRenderer.RenderMap(spawnAreaWidth, height, GenerateSpawnAreaNoiseMap());
        _noiseMapRenderer.RenderMap(spawnAreaWidth, height, GenerateSpawnAreaNoiseMap(), width - spawnAreaWidth);
    }

    private float[] GenerateSpawnAreaNoiseMap()
    {
        float[] noiseMap = NoiseMapGenerator.GenerateNoiseMap(spawnAreaWidth, height, scale, octaves, persistence,
            lacunarity,
            offset);
        var terrainLevels = _noiseMapRenderer.GetTerrainLevels();

        for (int i = 0; i < noiseMap.Length; i++)
        {
            if (noiseMap[i] <= terrainLevels[0].height)
            {
                noiseMap[i] = terrainLevels[1].height;
            }
        }

        return noiseMap;
    }

    private void GenerateBattleArea()
    {
        float[] noiseMap =
            NoiseMapGenerator.GenerateNoiseMap(BattleAreaWidth, height, scale, octaves, persistence, lacunarity,
                offset);

        _noiseMapRenderer.RenderMap(BattleAreaWidth, height, noiseMap, spawnAreaWidth);
    }
}