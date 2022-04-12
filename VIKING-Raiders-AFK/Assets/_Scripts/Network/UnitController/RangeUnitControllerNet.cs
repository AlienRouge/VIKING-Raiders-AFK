using _Scripts.Network.UnitController;
using UnityEngine;

public class RangeUnitControllerNet : BaseUnitControllerNet
{
    [SerializeField] private ProjectileController _projectilePrefab;

    protected override void DoOnAttack(int damage)
    {
        var newProjectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        /*newProjectile.transform.SetParent(transform.root);*/
        newProjectile.Init(_currentTarget, ActualStats.Model.GetProjModel(), damage);
        newProjectile.Launch();
    }
}