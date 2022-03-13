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
   
   
    private readonly Dictionary<Team, List<BaseUnitController>> _unitsByTeams = new Dictionary<Team, List<BaseUnitController>>();
    
    private void Start()
    {
        _instance = this;
    }
    
    private void Init()
    {
        _unitsByTeams.Add(Team.Team1, new List<BaseUnitController>());
        _unitsByTeams.Add(Team.Team2, new List<BaseUnitController>());
    }
    
     public void StartBattle(List<BaseUnitController> units)
     {
         Init();
        
         foreach (var unit in units)
         {
             unit.UnitDead += OnUnitDied;
             _unitsByTeams[unit.MyTeam].Add(unit);
         }

         foreach (var unit in units)
             unit.StartBattle();
     }

    public List<BaseUnitController> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? _unitsByTeams[Team.Team2] : _unitsByTeams[Team.Team1];
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