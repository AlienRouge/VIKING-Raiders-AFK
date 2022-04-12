using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private AbilityState _passiveAbilityState;
    [SerializeField] private AbilityState _activeAbilityState;
    [SerializeField] private BaseAbility _passiveAbility;
    [SerializeField] private BaseAbility _activeAbility;
    [SerializeField] private BaseUnitController parent;

    private AbilityState ActiveAbilityState
    {
        get => _activeAbilityState;
        set
        {
            _activeAbilityState = value;
            EventController.ActiveAbilityStateChanged?.Invoke(parent, value);
        }
    }

    private AbilityState PassiveAbilityState
    {
        get => _passiveAbilityState;
        set
        {
            _passiveAbilityState = value;
            EventController.PassiveAbilityStateChanged?.Invoke(parent, value);
        }
    }

    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    private void Awake()
    {
        parent = gameObject.GetComponent<BaseUnitController>();
    }
    
    public void InitPassiveAbility(BaseAbility passiveAbility)
    {
        _passiveAbility = passiveAbility;
        SetAbilityState(passiveAbility, AbilityState.Ready);
    }

    public void InitActiveAbility(BaseAbility activeAbility)
    {
        _activeAbility = activeAbility;
        SetAbilityState(_activeAbility, AbilityState.Ready);
    }

    public async Task ActivatePassiveAbility(BaseUnitController target)
    {
        var ability = _passiveAbility;
        await ActivateAbility(ability, target);
    }

    public async Task ActivateActiveAbility(BaseUnitController target)
    {
        var ability = _activeAbility;
        await ActivateAbility(ability, target);
    }

    private async Task ActivateAbility(BaseAbility ability, BaseUnitController target)
    {
        var abilityState = ability == _activeAbility ? ActiveAbilityState : PassiveAbilityState;
        if (abilityState == AbilityState.Ready)
        {
            SetAbilityState(ability, AbilityState.Active);
            await ability.OnActivate(parent, GetTargets(ability, target));

            AbilityCycleAsync(ability);
        }
    }


    private void SetAbilityState(BaseAbility ability, AbilityState state)
    {
        if (ReferenceEquals(ability, _activeAbility))
        {
            ActiveAbilityState = state;
        }
        else
        {
            PassiveAbilityState = state;
        }
    }


    public bool CheckPassiveAbilityRange(BaseUnitController target)
    {
        var ability = _passiveAbility;
        return GetTargets(ability, target).Count > 0;
    }

    private List<BaseUnitController> GetTargets(BaseAbility ability, BaseUnitController target)
    {
        List<BaseUnitController> targets;
        switch (ability.Target)
        {
            case BaseAbility.TargetType.CurrentTarget:
                targets = new List<BaseUnitController> { target };
                break;

            case BaseAbility.TargetType.Self:
                targets = new List<BaseUnitController> { parent };
                break;

            case BaseAbility.TargetType.MyTeam:
                targets = new List<BaseUnitController>(
                    BattleSceneController.instance.BattleController.GetFriendlies(parent.ActualStats.BattleTeam));
                break;

            case BaseAbility.TargetType.EnemyTeam:
                targets = new List<BaseUnitController>(
                    BattleSceneController.instance.BattleController.GetEnemies(parent.ActualStats.BattleTeam));
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            var distance = Vector3.Distance(parent.transform.position, targets[i].transform.position);
            if (distance > ability.CastRange)
            {
                targets.Remove(targets[i]);
            }
        }

        return targets;
    }

    private async Task AbilityCycleAsync(BaseAbility ability)
    {
        await ActivityAsync(ability);
        await CooldownAsync(ability);
    }

    private async Task ActivityAsync(BaseAbility ability)
    {
        // _ability.OnStartActivity(null);
        await Task.Delay((int)(ability.ActiveTime * Consts.ONE_SECOND_VALUE));
        // _ability.OnEndActivity(null);
        SetAbilityState(ability, AbilityState.Cooldown);
    }

    private async Task CooldownAsync(BaseAbility ability)
    {
        // _ability.OnStartCooldown(null);
        await Task.Delay((int)(ability.CooldownTime * Consts.ONE_SECOND_VALUE));
        // _ability.OnEndCooldown(null);
        SetAbilityState(ability, AbilityState.Ready);
    }
}