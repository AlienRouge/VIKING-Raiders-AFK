using UnityEngine;

public class RangeUnitController : BaseUnitController
{
    [SerializeField] private ProjectileController _projectilePrefab;

    protected override void DoOnAttack(int damage)
    {
        var newProjectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        newProjectile.transform.SetParent(transform.root);
        newProjectile.Init(_currentTarget, ActualStats.UnitModel.GetProjModel(), damage);
        newProjectile.Launch();
    }
}