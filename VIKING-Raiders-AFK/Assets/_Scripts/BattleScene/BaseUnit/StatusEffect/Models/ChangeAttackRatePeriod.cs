using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeAttackRatePeriod")]
public class ChangeAttackRatePeriod : BaseStatusEffect
{
    [SerializeField] private float _multiplier;
    protected override void DoOnApplyEffect(BaseUnitController target)
    {
        target.ActualStats.Damage *= _multiplier;
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
        target.ActualStats.RestoreDamageValue();
    }
}