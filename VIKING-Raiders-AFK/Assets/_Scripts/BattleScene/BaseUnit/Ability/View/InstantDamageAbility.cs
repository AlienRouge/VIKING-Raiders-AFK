using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/InstantDamage")]
public class InstantDamageAbility : BaseAbility
{
    [SerializeField] private MagicalDamageType _magicalDamageType;
    [SerializeField] private float baseDamage;
    
    protected override void DoOnActivate(BaseUnitController target)
    {
        target.ChangeHealth(-baseDamage);
    }
}