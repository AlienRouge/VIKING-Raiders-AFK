using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;

public class BattleController : MonoBehaviour
{
    private static BattleController _instance;
    public static BattleController instance
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
    private readonly Dictionary<Team, List<BaseUnitController>> _unitsByTeams = new Dictionary<Team, List<BaseUnitController>>();
    
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
        _unitsByTeams.Add(Team.Team1, new List<BaseUnitController>());
        _unitsByTeams.Add(Team.Team2, new List<BaseUnitController>());
    }
    
     public void StartBattle()
     {
         // Spawn units
         InstantiateUnits(Team.Team1, playerHeroes, playerSpawnPoints);
         InstantiateUnits(Team.Team2, enemyHeroes, enemySpawnPoints);
     }

    public List<BaseUnitController> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? _unitsByTeams[Team.Team2] : _unitsByTeams[Team.Team1];
    }

    private void InstantiateUnits(Team team, List<BaseUnitModel> heroes, Transform[] spawnPoints)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            BaseUnitController newUnit = Instantiate(unitPrefab);
            newUnit.UnitDead += OnUnitDied;
            _unitsByTeams[team].Add(newUnit);
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
        unit.UnitDead -= OnUnitDied; 
        _unitsByTeams[team] = _unitsByTeams[team]
            .Where(value => value.gameObject.GetInstanceID() != unit.gameObject.GetInstanceID()).ToList();
        
        /*Destroy(unit.gameObject);*/
        /*unit.gameObject.SetActive(false);*/
        unit.gameObject.SetActive(false);
        
        if (_unitsByTeams[Team.Team1].Count == 0 || _unitsByTeams[Team.Team2].Count == 0 )
        {
            EventController.BattleEnded?.Invoke();
        }
    }

}