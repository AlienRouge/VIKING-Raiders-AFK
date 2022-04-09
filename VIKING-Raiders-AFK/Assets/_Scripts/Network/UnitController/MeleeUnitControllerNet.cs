using _Scripts.Network.UnitController;
using UnityEngine;

public class MeleeUnitControllerNet : BaseUnitControllerNet
{
    protected override void DoOnAttack(int damage)
    {
        _currentTarget.ChangeHealth(-damage);
    }
}