using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "StatusEffects/PeriodicDamage")]
public class PeriodicDamage : BaseStatusEffect
{
    [field: SerializeField] public float TickDamage { get; private set; }

    protected override void DoOnApplyEffect(BaseUnitController target)
    {
    }

    protected override void DoOnTickEffect(BaseUnitController target)
    {
        target.ChangeHealth(-TickDamage);
    }

    protected override void DoOnRemoveEffect(BaseUnitController target)
    {
    }
}