using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseAbility : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float ActiveTime { get; private set; }
    [field: SerializeField] public float CastRange { get; private set; }
    [field: SerializeField] public float CastTime { get; private set; }

    [field: SerializeField] public TargetType Target { get; private set; }

    [Serializable]
    public enum TargetType
    {
        Self,
        CurrentTarget,
        MyTeam,
        EnemyTeam
    }
    // Pattern
    public async Task OnActivate(List<BaseUnitController> targets)
    {
        Debug.Log("Casting..." + AbilityName);
        await Task.Delay(Mathf.RoundToInt(CastTime * 1000));
        Debug.Log("Piu!");

        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnActivate(unit);
            }
        }
    }

    public void OnStartActivity(List<BaseUnitController> targets)
    {
        Debug.Log("Ability activity started.");
        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnStartActivity(unit);
            }
        }
    }

    public void OnEndActivity(List<BaseUnitController> targets)
    {
        Debug.Log("Ability activity started.");
        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnEndActivity(unit);
            }
        }
    }

    public void OnStartCooldown(List<BaseUnitController> targets)
    {
        Debug.Log("Ability activity started.");
        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnStartCooldown(unit);
            }
        }
    }

    public void OnEndCooldown(List<BaseUnitController> targets)
    {
        Debug.Log("Ability activity started.");
        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnEndCooldown(unit);
            }
        }
    }

    protected abstract void DoOnActivate(BaseUnitController target);

    protected abstract void DoOnStartActivity(BaseUnitController target);

    protected abstract void DoOnEndActivity(BaseUnitController target);

    protected abstract void DoOnStartCooldown(BaseUnitController target);

    protected abstract void DoOnEndCooldown(BaseUnitController target);
}