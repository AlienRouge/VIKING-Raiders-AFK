using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnContoller : MonoBehaviour
{
    private static SpawnContoller _instance;

    public static SpawnContoller Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SpawnContoller) + "is NULL!");

            return _instance;
        }
    }

    [SerializeField] private BaseUnitController _baseUnitPrefab;
    [SerializeField] private List<BaseUnitController> _spawnedUnits;
    [SerializeField] private List<Transform> _enemiesSpawnPoints;
    [SerializeField] private List<Transform> _playerSpawnPoints;

    private void Start()
    {
        _instance = this;
        _GetSpawnPointsMock(); // Replace Init() 
    }

    public void Init(List<Transform> playerSpawnPoints, List<Transform> enemiesSpawnPoints)
    {
        _playerSpawnPoints = playerSpawnPoints;
        _enemiesSpawnPoints = enemiesSpawnPoints;
    }


    public int SpawnUnit(BaseUnitModel unitModel)
    {
        Debug.Log("Spawned: " + unitModel.characterName);
        return InstantiateUnit(Team.Team1, unitModel, GetSpawnPoint());
    }

    public void RemoveUnit(BaseUnitModel unitModel, int unitInstanceID)
    {
        foreach (var unit in _spawnedUnits.Where(unit => unit.GetInstanceID() == unitInstanceID))
        {
            _spawnedUnits.Remove(unit);
            Debug.Log("Deleted: " + unit.characterName);
            Destroy(unit.gameObject);
            break;
        }
    }

    private Transform GetSpawnPoint()
    {
        var index = Random.Range(0, _playerSpawnPoints.Count);
        Debug.Log(index);
        return _playerSpawnPoints[index];
    }
    // private void InstantiateUnits(Team team, List<BaseUnitModel> heroes, Transform[] spawnPoints)
    // {
    //     for (int i = 0; i < heroes.Count; i++)
    //     {
    //         BaseUnitController newUnit = Instantiate(_baseUnitPrefab);
    //
    //         newUnit.UnitDead += OnUnitDied;
    //         _unitsByTeams[team].Add(newUnit);
    //         newUnit.Init(heroes[i], team, spawnPoints[i].position);
    //         newUnit.name = heroes[i].characterName;
    //         WaitInit(newUnit);
    //     }
    // }

    private int InstantiateUnit(Team team, BaseUnitModel unitModel, Transform spawnPoint)
    {
        BaseUnitController newUnit = Instantiate(_baseUnitPrefab, spawnPoint.position, Quaternion.identity);
        newUnit.Init(unitModel, team);
        newUnit.name = unitModel.characterName;
        _spawnedUnits.Add(newUnit);

        return newUnit.GetInstanceID();
    }
    

    private void _GetSpawnPointsMock()
    {
        _playerSpawnPoints = FindObjectOfType<PlayerSpawnPoint>().GetSpawnTransforms();
        _enemiesSpawnPoints = FindObjectOfType<EnemiesSpawnPoint>().GetSpawnTransforms();
    }
}