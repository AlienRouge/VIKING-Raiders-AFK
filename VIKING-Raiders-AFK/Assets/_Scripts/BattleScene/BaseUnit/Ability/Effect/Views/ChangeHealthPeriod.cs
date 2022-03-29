using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeHealthPeriod")]
public class ChangeHealthPeriod : BaseStatusEffect
{
    [field: SerializeField] public float TickAmount { get; private set; }
    
    protected override void DoOnApplyEffect(BaseUnitController target)
    {
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        target.ChangeHealth(TickAmount);
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
    }
}