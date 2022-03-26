using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/InstantHeal")]
public class InstantHealAbility : BaseAbility
{
    [SerializeField] private float healAmount;
    
    protected override void DoOnActivate(BaseUnitController target)
    {
        target.ChangeHealth(healAmount);
    }

    protected override void DoOnStartActivity(BaseUnitController target)
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEndActivity(BaseUnitController target)
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnStartCooldown(BaseUnitController target)
    {
        throw new System.NotImplementedException();
    }

    protected override void DoOnEndCooldown(BaseUnitController target)
    {
        throw new System.NotImplementedException();
    }
}
