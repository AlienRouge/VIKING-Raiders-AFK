using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;

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

    private SpawnPointController _spawnPointController;

    private struct SpawnedUnit
    {
        public int ButtonID;
        public BaseUnitController unitController;
        public SpawnPoint SpawnPoint;
    }

    public int PlayerTeamSize => _playerTeam.Count;

    [SerializeField] private BaseUnitController _baseUnitPrefab;

    [SerializeField] private List<SpawnedUnit> _playerTeam;
    [SerializeField] private List<BaseUnitController> _enemyTeam;

    private void Awake()
    {
        _instance = this;
        _playerTeam = new List<SpawnedUnit>();
    }

    public void Init(SpawnPointController spawnPointController)
    {
        _spawnPointController = spawnPointController;
    }

    public bool TrySpawnUnit(BaseUnitModel unitModel, int buttonID)
    {
        if (_playerTeam.Count >= MAX_PLAYER_TEAM_SIZE)
        {
            Debug.Log("Team overflow. " + unitModel.characterName);
            return false;
        }

        var sp = _spawnPointController.GetFreeSpawnPoints(Team.Team1);
        if (sp.Count <= 0)
        {
            Debug.Log("No free spawn points." + unitModel.characterName);
            return false;
        }

        Debug.Log("Spawned:" + unitModel.characterName);
        InstantiateUnit(Team.Team1, unitModel, sp[0], buttonID);
        return true;
    }

    public bool RemoveUnit(BaseUnitModel unitModel, int buttonID)
    {
        var unit = _playerTeam.Find(unit => unit.ButtonID == buttonID);
        if (unit.unitController == null)
        {
            Debug.Log(buttonID);
            return false;
        }

        _spawnPointController.FreeSpawnPoint(unit.SpawnPoint);
        _playerTeam.Remove(unit);

        BaseUnitController unitController = unit.unitController;
        Debug.Log("Deleted: " + unitController.characterName);
        Destroy(unitController.gameObject);
        return true;
    }

    public void SpawnEnemies(List<BaseUnitModel> enemiesModels)
    {
        foreach (var enemy in enemiesModels)
        {
            var freeSP = _spawnPointController.GetFreeSpawnPoints(Team.Team2);
            InstantiateUnit(Team.Team2, enemy, freeSP[0]);
        }
    }

    public List<BaseUnitController> GetSpawnedUnits()
    {
        return _playerTeam.Select(unit => unit.unitController).Concat(_enemyTeam).ToList();
    }


    private void InstantiateUnit(Team team, BaseUnitModel unitModel, SpawnPoint spawnPoint, int buttonID = -1)
    {
        BaseUnitController newUnit = Instantiate(_baseUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);

        newUnit.Init(unitModel, team);
        newUnit.name = unitModel.characterName;

        if (team == Team.Team1)
        {
            _playerTeam.Add(new SpawnedUnit
            {
                ButtonID = buttonID, 
                unitController = newUnit, 
                SpawnPoint = spawnPoint
            });
        }
        else
        {
            _enemyTeam.Add(newUnit);
        }
    }
}