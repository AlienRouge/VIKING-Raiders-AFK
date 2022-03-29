using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseAbility : ScriptableObject
{
    [field: SerializeField] public TargetType Target { get; private set; }
    [field: SerializeField] public string AbilityName { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float ActiveTime { get; private set; }
    [field: SerializeField] public float CastRange { get; private set; }
    [field: SerializeField] public float CastTime { get; private set; }

    [field: SerializeField] public BaseStatusEffect StatusEffect { get; private set; }

    [field: SerializeField, Range(0, 1)] public float StatusEffectProcRate { get; private set; }

    [Serializable]
    public enum TargetType
    {
        Self,
        CurrentTarget,
        MyTeam,
        EnemyTeam
    }

    // Pattern
    public async Task OnActivate(BaseUnitController parent, List<BaseUnitController> targets)
    {
        Debug.Log("Casting..." + AbilityName);
        await Task.Delay(Mathf.RoundToInt(CastTime * 1000));

        if (parent.isDead) return;
        foreach (var unit in targets)
        {
            if (!unit.isDead)
            {
                DoOnActivate(unit);
                if (StatusEffect && Random.Range(0, 1) - StatusEffectProcRate >= 0)
                {
                    unit.AddStatusEffect(StatusEffect);
                }
            }
        }
    }

    public void OnStartActivity(BaseUnitController parent, List<BaseUnitController> targets)
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

    public void OnEndActivity(BaseUnitController parent, List<BaseUnitController> targets)
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

    public void OnStartCooldown(BaseUnitController parent, List<BaseUnitController> targets)
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

    public void OnEndCooldown(BaseUnitController parent, List<BaseUnitController> targets)
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