using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using _Scripts.UnitScripts.Views;
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
    
    [SerializeField] private BaseUnitView unitPrefab;
    [SerializeField] private List<BaseUnitModel> playerHeroes;
    [SerializeField] private List<BaseUnitModel> enemyHeroes;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] playerSpawnPoints;
    private readonly Dictionary<Team, List<BaseUnitView>> unitsByTeams = new Dictionary<Team, List<BaseUnitView>>();

    private bool _isEnd = false;
    
    private void Awake()
    {
        _instance = this;
        playerSpawnPoints = FindObjectOfType<PlayerSpawnPoint>().GetSpawnTransforms();
        enemySpawnPoints = FindObjectOfType<EnemySpawnPoint>().GetSpawnTransforms();

        Init();

    }

    private void Start()
    {
        StartBattle();
    }
    private void OnEnable()
    {
        EventController.UnitDied += OnUnitDied;
        /*EventController.UnitSpawned += OnUnitSpawned;*/
    }

    private void OnDisable()
    {        
        EventController.UnitDied -= OnUnitDied;
        /*EventController.UnitSpawned -= OnUnitSpawned;*/
    }
    
    public void Init()
    {
        // Create 2 teams
        unitsByTeams.Add(Team.Team1, new List<BaseUnitView>());
        unitsByTeams.Add(Team.Team2, new List<BaseUnitView>());
    }

    
    //Вынести в отдельный контроллер
    public void StartBattle()
    {
        // Spawn units
        InstantiateUnits(Team.Team1, playerHeroes, playerSpawnPoints);
        InstantiateUnits(Team.Team2, enemyHeroes, enemySpawnPoints);

        foreach (var units in unitsByTeams)
        {
            foreach (var unit in units.Value)
            {
                StartUnitBattle(unit);
            }
        }
    }

    public List<BaseUnitView> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? unitsByTeams[Team.Team2] : unitsByTeams[Team.Team1];
    }

    private void InstantiateUnits(Team team, List<BaseUnitModel> heroes, Transform[] spawnPoints)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            BaseUnitView newUnit = Instantiate(unitPrefab);
            newUnit.Init(heroes[i], team, spawnPoints[i].position);
            newUnit.name = newUnit.characterName;
            newUnit.EndFight += OnEndFight; 
            unitsByTeams[team].Add(newUnit);
            waitInit(newUnit);
        }
    }



    private async void waitInit(BaseUnitView newUnit)
    {
        await Task.Delay(10);
        if (newUnit != null)
        {
            newUnit.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    private void StartUnitBattle(BaseUnitView spawnedUnit)
    {
        if (_isEnd) return;
        if (spawnedUnit.CanFindTarget())
        {
            spawnedUnit.AttackTarget();
        }
    }
    
    private void OnEndFight(BaseUnitView unit)
    {
        if (_isEnd) return;
        StartUnitBattle(unit);
    }
    private void OnUnitDied(Team team, BaseUnitView unit)
    {
        unitsByTeams[team] = unitsByTeams[team]
            .Where(value => value.gameObject.GetInstanceID() != unit.gameObject.GetInstanceID()).ToList();
        unit.EndFight -= OnEndFight;
        unit.gameObject.SetActive(false);
        Destroy(unit.gameObject);
        if (unitsByTeams[Team.Team1].Count == 0 || unitsByTeams[Team.Team2].Count == 0 )
        {
            _isEnd = true;
            EventController.GameEnded?.Invoke();
        }
    }

}