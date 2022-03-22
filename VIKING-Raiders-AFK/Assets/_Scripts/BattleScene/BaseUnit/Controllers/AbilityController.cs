using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class AbilityController : MonoBehaviour
{
    private BaseAbility _ability;
    private BaseAbility _activeAbility;
    private float _cooldownTime;
    private float _activeTime;

    private enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    public void Init(BaseAbility ability)
    {
        _ability = ability;
        _state = AbilityState.Ready;
    }

    [SerializeField] private AbilityState _state;
    public UnityAction AbilityReady;

    public async Task ActivateAbility(BaseUnitController parent, BaseUnitController target)
    {
        if (_state == AbilityState.Ready)
        {
            _state = AbilityState.Active;
            await _ability.OnActivate(GetTargets(parent, target));

            AbilityCycleAsync();
        }
    }
    
    
    public bool CheckAbilityRange(BaseUnitController parent, BaseUnitController target)
    {
        return GetTargets(parent, target).Count>0;
    }

    private List<BaseUnitController> GetTargets(BaseUnitController parent, BaseUnitController target)
    {
        List<BaseUnitController> targets;
        switch (_ability.Target)
        {
            case BaseAbility.TargetType.CurrentTarget:
                targets = new List<BaseUnitController> { target };
                break;
              
            case BaseAbility.TargetType.Self:
                targets = new List<BaseUnitController> { parent };
                break;
                
            case BaseAbility.TargetType.MyTeam:
                targets = new List<BaseUnitController>(BattleController.instance.GetFriendlies(parent.MyTeam));
                break;
               
            case BaseAbility.TargetType.EnemyTeam:
                targets = new List<BaseUnitController>(BattleController.instance.GetEnemies(parent.MyTeam));
                break;
               
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            var distance = Vector3.Distance(parent.transform.position, targets[i].transform.position);
            if (distance > _ability.CastRange)
            {
                targets.Remove(targets[i]);
            }
        }
        
        return targets;
    }

    private async Task AbilityCycleAsync()
    {
        await ActivityAsync(_ability.ActiveTime);
        await CooldownAsync(_ability.CooldownTime);
        AbilityReady?.Invoke();
    }

    private async Task ActivityAsync(float time)
    {
        // _ability.OnStartActivity(null);
        await Task.Delay((int)(time * 1000f));
        // _ability.OnEndActivity(null);
        _state = AbilityState.Cooldown;
    }

    private async Task CooldownAsync(float time)
    {
        // _ability.OnStartCooldown(null);
        await Task.Delay((int)(time * 1000f));
        // _ability.OnEndCooldown(null);
        _state = AbilityState.Ready;
    }
}