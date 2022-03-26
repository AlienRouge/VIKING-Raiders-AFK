using _Scripts.Enums;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    protected static MapGenerator _instance;
    
    [Header("Map settings")] [SerializeField]
    protected MapController _mapPrefab;

    [SerializeField] protected int width;
    [SerializeField] protected int height;
    [SerializeField] protected int spawnAreaWidth;
    [SerializeField] private Vector2 spawnAreaOffset;

    [Header("Generation settings")] [SerializeField]
    private float scale;

    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;
    [SerializeField] private Vector2 offset;

    protected MapController _mapController;
    protected int BattleAreaWidth => width - 2 * spawnAreaWidth;
    protected NoiseMapRenderer _noiseMapRenderer;

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
            _mapController.transform.localPosition = new Vector3(
                -width/2f,
                -height/2f, 1);
        }
        
        _noiseMapRenderer.Init(_mapController.walkableTilemap, _mapController.notWalkableTilemap,
            _mapController.decorTilemap);
    }

    public virtual MapController GenerateMap()
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

    protected void GenerateSpawnAreas()
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

    protected float[] GetNoiseMap()
    {
        return NoiseMapGenerator.GenerateNoiseMap(BattleAreaWidth, height, scale, octaves, persistence, lacunarity,
            offset);
    }

    protected virtual void GenerateBattleArea()
    {
        float[] noiseMap = GetNoiseMap();
        
        _noiseMapRenderer.RenderMap(BattleAreaWidth, height, noiseMap, spawnAreaWidth);
    }
}