using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;

public class SpawnController : MonoBehaviourPun
{
    protected SpawnPointsController spawnPointsController;
    public Team CurrentTeam {  get; protected set; }

    protected struct SpawnedUnit
    {
        public int ButtonID;
        public BaseUnitController unitController;
        public SpawnPoint SpawnPoint;
    }

    [SerializeField] protected BaseUnitController _baseMeleeUnitPrefab;
    [SerializeField] protected BaseUnitController _baseRangeUnitPrefab;

    [SerializeField] protected List<SpawnedUnit> _playerTeam;
    [SerializeField] protected List<BaseUnitController> _enemyTeam;

    protected delegate void SpawnAction(Team team, Hero unitModel, SpawnPoint spawnPoint, int buttonID = -1);

    protected SpawnAction SpawnUnit;
    public int PlayerTeamSize => _playerTeam.Count;
    public int EnemyTeamSize => _enemyTeam.Count;

    private void Awake()
    {
        _playerTeam = new List<SpawnedUnit>();
    }

    private void Start()
    {
        SpawnUnit = InstantiateUnit;
        CurrentTeam = Team.Team1;
    }

    public void Init(SpawnPointsController spawnPointsController)
    {
        this.spawnPointsController = spawnPointsController;
    }

    public bool TrySpawnUnit(Hero hero, int buttonID)
    {
        if (_playerTeam.Count >= Consts.MAX_PLAYER_TEAM_SIZE)
        {
            Debug.Log("Team overflow. " + hero._heroModel.CharacterName);
            return false;
        }

        var sp = spawnPointsController.GetFreeSpawnPoints(CurrentTeam);
        if (sp.Count <= 0)
        {
            Debug.Log("No free spawn points." + hero._heroModel.CharacterName);
            return false;
        }

        Debug.Log("Spawned:" + hero._heroModel.CharacterName);
        SpawnUnit(CurrentTeam, hero, sp[0], buttonID);
        return true;
    }

    public virtual bool TryRemoveUnit(Hero unitModel, int buttonID)
    {
        var unit = _playerTeam.Find(unit => unit.ButtonID == buttonID);
        if (unit.unitController == null)
        {
            return false;
        }

        spawnPointsController.FreeSpawnPoint(unit.SpawnPoint);
        _playerTeam.Remove(unit);

        BaseUnitController unitController = unit.unitController;
        Debug.Log("Deleted: " + unitController.ActualStats.Model.CharacterName);
        Destroy(unitController.gameObject);
        return true;
    }

    public void SpawnEnemies(List<Hero> enemyHeroes)
    {
        foreach (var enemy in enemyHeroes)
        {
            var freeSP = spawnPointsController.GetFreeSpawnPoints(Team.Team2);
            InstantiateUnit(Team.Team2, enemy, freeSP[0]);
        }
    }

    public List<BaseUnitController> GetSpawnedUnits()
    {
        return _playerTeam.Select(unit => unit.unitController).Concat(_enemyTeam).ToList();
    }

    public List<BaseUnitController> GetPlayerUnits()
    {
        return _playerTeam.Select(unit => unit.unitController).ToList();
    }


    private void InstantiateUnit(Team team, Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        BaseUnitController newUnit;

        switch (hero._heroModel)
        {
            case BaseMeleeUnitModel _:
                newUnit = Instantiate(_baseMeleeUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
                break;
            case BaseRangeUnitModel _:
                newUnit = Instantiate(_baseRangeUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
                break;
            default:
                throw new ArgumentException("WrongUnitType");
        }

        spawnPointsController.TakeSpawnPoint(spawnPoint);
        newUnit.Init(hero._heroModel, team, hero._heroLevel, team == Team.Team1);
        newUnit.name = hero._heroModel.CharacterName;
        newUnit.transform.SetParent(spawnPointsController.transform);
        if (team == Team.Team2)
        {
            newUnit.transform.Rotate(new Vector3(0, 180, 0));
        }

        if (team == CurrentTeam)
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