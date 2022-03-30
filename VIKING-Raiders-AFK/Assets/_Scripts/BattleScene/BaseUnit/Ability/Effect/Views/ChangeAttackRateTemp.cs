using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeAttackRateTemp")]
public class ChangeAttackRateTemp : BaseStatusEffect
{
    [SerializeField] private float _multiplier;
    private float _oldMultiplier;

    protected override void DoOnApplyEffect(BaseUnitController target)
    {
        _oldMultiplier = target.ActualStats.DmgMultiplier;
        target.ActualStats.DmgMultiplier = _multiplier;
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        ;
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
        target.ActualStats.DmgMultiplier = _oldMultiplier;
    }
}