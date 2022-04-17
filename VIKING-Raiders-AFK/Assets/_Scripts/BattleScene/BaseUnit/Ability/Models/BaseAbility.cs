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

        if (parent.ActualStats.IsDead) return;
        foreach (var unit in targets)
        {
            if (!unit.ActualStats.IsDead)
            {
                DoOnActivate(unit);
                if (StatusEffect &&  StatusEffectProcRate - Random.Range(0, 1) >= 0)
                {
                    unit.AddStatusEffect(StatusEffect);
                }
            }
        }
    }
    
    protected abstract void DoOnActivate(BaseUnitController target);
}