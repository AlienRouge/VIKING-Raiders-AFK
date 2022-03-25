using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/InstantDamage")]
public class MagicAbility : BaseAbility
{
    [SerializeField] private MagicalDamageType _magicalDamageType;
    [SerializeField] private float baseDamage;
    
    protected override void DoOnActivate(BaseUnitController target)
    {
        target.ChangeHealth(-baseDamage);
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