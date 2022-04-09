using UnityEngine;

public class RangeUnitController : BaseUnitController
{
    [SerializeField] private ProjectileController _projectilePrefab;

    protected override void InitializeAttackType()
    {
        Debug.Log("Init"); // TODO
    }

    protected override void DoOnAttack(int damage)
    {
        Debug.Log("RANGE ATTACK");
        var proj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        proj.transform.SetParent(transform.root);
        proj.Fire(ActualStats.UnitModel.GetProjModel(), _currentTarget, ActualStats.GetDamageValue());
    }
}
