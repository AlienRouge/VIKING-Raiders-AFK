using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    private void Awake()
    {
        _spawnPoints = FindObjectsOfType<SpawnPoint>().ToList();
    }

    public List<SpawnPoint> GetFreeSpawnPoints(Team team)
    {
        return _spawnPoints.Where(sp => (sp.Team == team && !sp.IsTaken)).ToList();
    }

    public List<SpawnPoint> GetTakenSpawnPoints(Team team)
    {
        return _spawnPoints.Where(sp => (sp.Team == team && sp.IsTaken)).ToList();
    }

    public bool TakeSpawnPoint(SpawnPoint spawnPoint)
    {
        var sp = _spawnPoints.Find(sp => sp == spawnPoint);
        if (sp.IsTaken)
            return false;

        sp.IsTaken = true;
        return true;
    }

    public bool FreeSpawnPoint(SpawnPoint spawnPoint)
    {
        var sp = _spawnPoints.Find(sp => sp == spawnPoint);

        if (!sp.IsTaken)
            return false;

        sp.IsTaken = false;
        return true;
    }
}