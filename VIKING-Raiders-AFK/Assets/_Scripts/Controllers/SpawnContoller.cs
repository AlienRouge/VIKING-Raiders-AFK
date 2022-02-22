using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnContoller : MonoBehaviour
{
    private const int MAX_PLAYER_TEAM_SIZE = 4;

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
    [SerializeField] private List<BaseUnitController> _playerTeam;
    [SerializeField] private List<BaseUnitController> _enemyTeam;

    private SpawnPointController _spawnPointController;

    private void Awake()
    {
        _instance = this;
    }

    public void Init(SpawnPointController spawnPointController)
    {
        _spawnPointController = spawnPointController;
    }


    public int SpawnUnit(BaseUnitModel unitModel)
    {
        if (_playerTeam.Count < MAX_PLAYER_TEAM_SIZE)
        {
            var sp = _spawnPointController.GetFreeSpawnPoints(Team.Team1);
            if (sp == null)
            {
                Debug.Log("No free spawn points." + unitModel.characterName);
                return 0;
            }

            Debug.Log("Spawned:" + unitModel.characterName);
            return InstantiateUnit(Team.Team1, unitModel, sp[0]);
        }

        Debug.Log("Team overflow. " + unitModel.characterName);
        return 0;
    }


    public void RemoveUnit(BaseUnitModel unitModel, int unitInstanceID)
    {
        var deletedUnit = _playerTeam.Find(unit => unit.GetInstanceID() == unitInstanceID);
        _playerTeam.Remove(deletedUnit);

        var freeSp = _spawnPointController.GetTakenSpawnPoints(Team.Team1)
            .Find(sp => sp.GetPosition() == deletedUnit.transform.position);
        _spawnPointController.FreeSpawnPoint(freeSp);

        Debug.Log("Deleted: " + deletedUnit.characterName);
        Destroy(deletedUnit.gameObject);
    }


    // public void SpawnEnemies(List<BaseUnitModel> enemiesModels)
    // {
    //  InstantiateUnits(Team.Team2, enemiesModels, _enemiesSpawnPoints);   
    // }
    // private void InstantiateUnits(Team team, List<BaseUnitModel> unitModels, List<Transform> spawnPoints)
    // {
    //     for (int i = 0; i < unitModels.Count; i++)
    //     {
    //         InstantiateUnit(team, unitModels[i], spawnPoints[i]);
    //     }
    // }

    private int InstantiateUnit(Team team, BaseUnitModel unitModel, SpawnPoint spawnPoint)
    {
        BaseUnitController newUnit = Instantiate(_baseUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);

        newUnit.Init(unitModel, team);
        newUnit.name = unitModel.characterName;
        _playerTeam.Add(newUnit);

        return newUnit.GetInstanceID();
    }
}