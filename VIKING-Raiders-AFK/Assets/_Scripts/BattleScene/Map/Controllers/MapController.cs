using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private SpawnAreasController _spawnAreasController;
    private SpawnPointsController _spawnPointsController;
    
    [SerializeField] public NavMeshSurface2d _navMeshSurface2d;
    [SerializeField] public Tilemap walkableTilemap;
    [SerializeField] public Tilemap notWalkableTilemap;
    [SerializeField] public Tilemap decorTilemap;

    public SpawnAreasController SpawnAreasController => _spawnAreasController;
    public SpawnPointsController SpawnPointsController => _spawnPointsController;

    private void Awake()
    {
        _spawnAreasController = GetComponent<SpawnAreasController>();
        _spawnPointsController = GetComponent<SpawnPointsController>();
        transform.localPosition = new Vector3(
            -16 / 2f,
            -10 / 2f,
            1);
    }

    public void BakeMap()
    {
        _navMeshSurface2d.BuildNavMeshAsync();
    }
}