using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;

public class SpawnController : MonoBehaviourPun
{
    private static SpawnController _instance;

    public static SpawnController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SpawnController) + "is NULL!");

            return _instance;
        }
    }

    protected SpawnPointController _spawnPointController;
    protected Team _currentTeam;
    protected struct SpawnedUnit
    {
        public int ButtonID;
        public BaseUnitController unitController;
        public SpawnPoint SpawnPoint;
    }

    [SerializeField] protected BaseUnitController _baseUnitPrefab;

    [SerializeField] protected List<SpawnedUnit> _playerTeam;
    [SerializeField] protected List<BaseUnitController> _enemyTeam;

    protected delegate void SpawnAction(Team team, User.Hero unitModel, SpawnPoint spawnPoint, int buttonID = -1);
    protected SpawnAction SpawnUnit;
    public int PlayerTeamSize => _playerTeam.Count;
    public int EnemyTeamSize => _enemyTeam.Count;
    
    private void Awake()
    {
        _instance = this;
        _playerTeam = new List<SpawnedUnit>();
    }

    private void Start()
    {
        SpawnUnit = InstantiateUnit;
        _currentTeam = Team.Team1;
    }

    public void Init(SpawnPointController spawnPointController)
    {
        _spawnPointController = spawnPointController;
    }

    public bool TrySpawnUnit(User.Hero hero, int buttonID)
    {
        if (_playerTeam.Count >= Consts.MAX_PLAYER_TEAM_SIZE)
        {
            Debug.Log("Team overflow. " + hero._heroModel.CharacterName);
            return false;
        }

        var sp = _spawnPointController.GetFreeSpawnPoints(_currentTeam);
        if (sp.Count <= 0)
        {
            Debug.Log("No free spawn points." + hero._heroModel.CharacterName);
            return false;
        }

        Debug.Log("Spawned:" + hero._heroModel.CharacterName);
        SpawnUnit(_currentTeam, hero, sp[0], buttonID);
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
    
    public List<BaseUnitController> GetPlayerUnits()
    {
        return _playerTeam.Select(unit => unit.unitController).ToList();
    }


    private void InstantiateUnit(Team team, User.Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        BaseUnitController newUnit = Instantiate(_baseUnitPrefab, spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);

        newUnit.Init(hero._heroModel, team,  hero._heroLevel, team==Team.Team1);
        newUnit.name = hero._heroModel.CharacterName;
        newUnit.transform.SetParent(_spawnPointController.transform);

        if (team == _currentTeam)
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