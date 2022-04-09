using UnityEngine;

public class MeleeUnitController : BaseUnitController
{
    protected override void InitializeAttackType()
    {
        
    }

    protected override void DoOnAttack(int damage)
    {
        _currentTarget.ChangeHealth(-damage);
    }
}
