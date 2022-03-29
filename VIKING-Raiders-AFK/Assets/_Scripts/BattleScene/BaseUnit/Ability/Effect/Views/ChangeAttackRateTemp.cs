using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeAttackRateTemp")]
public class ChangeAttackRateTemp : BaseStatusEffect
{
    [SerializeField] private float _multiplier;

    protected override void DoOnApplyEffect(BaseUnitController target)
    {
        target.ChangeDamageRatioMultiplier(_multiplier);
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        ;
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
        target.ChangeDamageRatioMultiplier(1.0f);   //TODO RESTORE VALUE IN UNIT DATA CLASS
    }
}