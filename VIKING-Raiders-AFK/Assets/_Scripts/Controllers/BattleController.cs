using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;

public class BattleController : MonoBehaviour
{
    private static BattleController _instance;
    public static BattleController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleController) + "is NULL!");

            return _instance;
        }
    }
    
    [SerializeField] private BaseUnitController unitPrefab;
    [SerializeField] private List<BaseUnitModel> playerHeroes;
    [SerializeField] private List<BaseUnitModel> enemyHeroes;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] playerSpawnPoints;
    private readonly Dictionary<Team, List<BaseUnitController>> unitsByTeams = new Dictionary<Team, List<BaseUnitController>>();
    
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
    private void OnEnable()
    {
        EventController.UnitDied += OnUnitDied;
    }
    
    private void OnDisable()
    {        
        EventController.UnitDied -= OnUnitDied;
    }
    
    public void Init()
    {
        unitsByTeams.Add(Team.Team1, new List<BaseUnitController>());
        unitsByTeams.Add(Team.Team2, new List<BaseUnitController>());
    }
    
     public void StartBattle()
     {
         // Spawn units
         InstantiateUnits(Team.Team1, playerHeroes, playerSpawnPoints);
         InstantiateUnits(Team.Team2, enemyHeroes, enemySpawnPoints);
     }

    public List<BaseUnitController> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? unitsByTeams[Team.Team2] : unitsByTeams[Team.Team1];
    }

    private void InstantiateUnits(Team team, List<BaseUnitModel> heroes, Transform[] spawnPoints)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            BaseUnitController newUnit = Instantiate(unitPrefab);
            unitsByTeams[team].Add(newUnit);
            newUnit.Init(heroes[i], team, spawnPoints[i].position);
            newUnit.name = heroes[i].characterName;
            WaitInit(newUnit);
        }
    }
    
    private async void WaitInit(BaseUnitController newUnit)
    {
        await Task.Delay(10);
        if (newUnit != null)
        {
            newUnit.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    
    private void OnUnitDied(Team team, BaseUnitController unit)
    {
        unitsByTeams[team] = unitsByTeams[team]
            .Where(value => value.gameObject.GetInstanceID() != unit.gameObject.GetInstanceID()).ToList();
        
        Destroy(unit.gameObject);
        
        if (unitsByTeams[Team.Team1].Count == 0 || unitsByTeams[Team.Team2].Count == 0 )
        {
            EventController.GameEnded?.Invoke();
        }
    }

}