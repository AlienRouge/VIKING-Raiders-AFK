using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;


public class BattleController : MonoBehaviour
{
    protected readonly Dictionary<Team, List<BaseUnitController>> _unitsByTeams =
        new Dictionary<Team, List<BaseUnitController>>();

    public Team? WinnerTeam { get; private set; }
    
    protected void Init()
    {
        _unitsByTeams.Add(Team.Team1, new List<BaseUnitController>());
        _unitsByTeams.Add(Team.Team2, new List<BaseUnitController>());
    }

    public virtual void StartBattle(List<BaseUnitController> units)
    {
        Init();

        foreach (var unit in units)
        {
            _unitsByTeams[unit.ActualStats.BattleTeam].Add(unit);
        }

        foreach (var unit in units)
            unit.StartBattle();
    }

    public List<BaseUnitController> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Team1 ? _unitsByTeams[Team.Team2] : _unitsByTeams[Team.Team1];
    }

    public List<BaseUnitController> GetFriendlies(Team myTeam)
    {
        return _unitsByTeams[myTeam];
    }

    public void OnUnitDied(BaseUnitController unit)
    {
        _unitsByTeams[unit.ActualStats.BattleTeam] = _unitsByTeams[unit.ActualStats.BattleTeam]
            .Where(value => !ReferenceEquals(value, unit)).ToList();

        unit.gameObject.SetActive(false);
        if (_unitsByTeams[Team.Team1].Count == 0 || _unitsByTeams[Team.Team2].Count == 0)
        {
            OnBattleEnded();
        }
    }

    private void OnBattleEnded()
    {
        if (_unitsByTeams[Team.Team1].Count == 0 )
        {
            WinnerTeam = Team.Team2;
        }
        else if (_unitsByTeams[Team.Team2].Count == 0)
        {
            WinnerTeam = Team.Team1;
        }
       
        EventController.BattleEnded.Invoke();
    }

    public BaseUnitController GetTarget(BaseUnitController unit)
    {
        BaseUnitController supposedEnemy = null;
        var enemies = GetEnemies(unit.ActualStats.BattleTeam);
        var weights = unit.ActualStats.Model.TargetWeights;
        float enemyValue = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float distance = unit.GetDistanceToPosition(enemy.transform.position);
            float hp = enemy.ActualStats.Health;
            float mean = GetTargetWeightedMean(hp, distance, weights.HpWeight, weights.DistanceWeight);

            if (mean <= enemyValue)
            {
                enemyValue = mean;
                supposedEnemy = enemy;
            }
        }

        return supposedEnemy;
    }

    private float GetTargetWeightedMean(float hp, float distance, float hpWeight, float distanceWeight)
    {
        return (hpWeight * hp + distanceWeight * distance) / (hpWeight + distanceWeight);
    }
}