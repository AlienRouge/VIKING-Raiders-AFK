using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/InstantHeal")]
public class InstantHealAbility : BaseAbility
{
    [SerializeField] private float healAmount;
    
    protected override void DoOnActivate(BaseUnitController target)
    {
        target.ChangeHealth(healAmount);
    }
}
