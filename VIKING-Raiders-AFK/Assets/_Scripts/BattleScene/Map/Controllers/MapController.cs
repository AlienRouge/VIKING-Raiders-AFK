using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    [SerializeField] public NavMeshSurface2d _navMeshSurface2d;

    public SpawnAreaController spawnAreaController;
    public SpawnPointController spawnPointController;
    [SerializeField] public Tilemap walkableTilemap;
    [SerializeField] public Tilemap notWalkableTilemap;
    [SerializeField] public Tilemap decorTilemap;

    private void Awake()
    {
        spawnAreaController = GetComponent<SpawnAreaController>();
        spawnPointController = GetComponent<SpawnPointController>();
    }
    
    
    public void BakeMap()
    {
        _navMeshSurface2d.BuildNavMeshAsync();
    }
}
