using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeMoveSpeedPeriod")]
public class ChangeMoveSpeedPeriod : BaseStatusEffect
{
    [SerializeField] private float multiplier;
    protected override void DoOnApplyEffect(BaseUnitController target)
    {
        target.ActualStats.MoveSpeed *= multiplier;
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
        target.ActualStats.RestoreMoveSpeedValue();
    }
}
