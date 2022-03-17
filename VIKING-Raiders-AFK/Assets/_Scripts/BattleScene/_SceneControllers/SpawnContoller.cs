using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;

public class SpawnContoller : MonoBehaviour
{
    protected const int MAX_PLAYER_TEAM_SIZE = 4;

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

    protected SpawnPointController _spawnPointController;

    protected struct SpawnedUnit
    {
        public int ButtonID;
        public BaseUnitController unitController;
        public SpawnPoint SpawnPoint;
    }

    public int PlayerTeamSize => _playerTeam.Count;

    [SerializeField] protected BaseUnitController _baseUnitPrefab;

    [SerializeField] protected List<SpawnedUnit> _playerTeam;
    [SerializeField] protected List<BaseUnitController> _enemyTeam;

    protected delegate void SpawnAction(Team team, User.Hero unitModel, SpawnPoint spawnPoint, int buttonID = -1);
    protected SpawnAction SpawnUnit;
    
    private void Awake()
    {
        _instance = this;
        _playerTeam = new List<SpawnedUnit>();
    }

    private void Start()
    {
        SpawnUnit = InstantiateUnit;
    }

    public void Init(SpawnPointController spawnPointController)
    {
        _spawnPointController = spawnPointController;
    }

    public bool TrySpawnUnit(User.Hero hero, int buttonID)
    {
        if (_playerTeam.Count >= MAX_PLAYER_TEAM_SIZE)
        {
            Debug.Log("Team overflow. " + hero._heroModel.CharacterName);
            return false;
        }

        var sp = _spawnPointController.GetFreeSpawnPoints(Team.Team1);
        if (sp.Count <= 0)
        {
            Debug.Log("No free spawn points." + hero._heroModel.CharacterName);
            return false;
        }

        Debug.Log("Spawned:" + hero._heroModel.CharacterName);
        SpawnUnit(Team.Team1, hero, sp[0], buttonID);
        return true;
    }

    public bool RemoveUnit(User.Hero unitModel, int buttonID)
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

    public void SpawnEnemies(List<User.Hero> enemyHeroes)
    {
        foreach (var enemy in enemyHeroes)
        {
            var freeSP = _spawnPointController.GetFreeSpawnPoints(Team.Team2);
            InstantiateUnit(Team.Team2, enemy, freeSP[0]);
        }
    }

    public List<BaseUnitController> GetSpawnedUnits()
    {
        return _playerTeam.Select(unit => unit.unitController).Concat(_enemyTeam).ToList();
    }


    private void InstantiateUnit(Team team, User.Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        BaseUnitController newUnit = Instantiate(_baseUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);

        newUnit.Init(hero._heroModel, team,  hero._heroLevel, team==Team.Team1);
        newUnit.name = hero._heroModel.CharacterName;

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