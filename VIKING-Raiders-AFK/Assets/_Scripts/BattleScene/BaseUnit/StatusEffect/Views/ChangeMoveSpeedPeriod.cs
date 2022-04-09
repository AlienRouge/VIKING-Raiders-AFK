using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/ChangeMoveSpeedPeriod")]
public class ChangeMoveSpeedPeriod : BaseStatusEffect
{
    [SerializeField] private float multiplier;
    protected override void DoOnApplyEffect(BaseUnitController target)
    {
        target.ChangeMoveSpeed(multiplier);
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
        target.ChangeMoveSpeed(1f);
    }
}
