using System.Collections.Generic;
using System.Linq;
using UnitScripts;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleManager) + "is NULL!");

            return _instance;
        }
    }
    
    [SerializeField] private BaseUnit unitPrefab;
    [SerializeField] private List<Hero> playerHeroes;
    [SerializeField] private List<Hero> enemyHeroes;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] playerSpawnPoints;
    private readonly Dictionary<Team, List<BaseUnit>> unitsByTeams = new Dictionary<Team, List<BaseUnit>>();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        playerSpawnPoints = FindObjectOfType<PlayerSpawnPoint>().GetSpawnTransforms();
        enemySpawnPoints = FindObjectOfType<EnemySpawnPoint>().GetSpawnTransforms();

        Init();
        StartBattle();
    }

    public void Init()
    {
        // Create 2 teams
        unitsByTeams.Add(Team.Team1, new List<BaseUnit>());
        unitsByTeams.Add(Team.Team2, new List<BaseUnit>());
    }

    public void StartBattle()
    {
        // Spawn units
        InstantiateUnits(Team.Team1, playerHeroes, playerSpawnPoints);
        InstantiateUnits(Team.Team2, enemyHeroes, enemySpawnPoints);
    }

    public void UnitDied(Team team, BaseUnit unit)
    {
        unitsByTeams[team] = unitsByTeams[team]
            .Where(value => value.gameObject.GetInstanceID() != unit.gameObject.GetInstanceID()).ToList();
        Destroy(unit.gameObject);
    }

    public List<BaseUnit> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? unitsByTeams[Team.Team2] : unitsByTeams[Team.Team1];
    }

    private void InstantiateUnits(Team team, List<Hero> heroes, Transform[] spawnPoints)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            BaseUnit newUnit = Instantiate(unitPrefab);
            unitsByTeams[team].Add(newUnit);
            newUnit.Init(team, spawnPoints[i], heroes[i]);
        }
    }

    public enum Team
    {
        Team1,
        Team2
    }
}