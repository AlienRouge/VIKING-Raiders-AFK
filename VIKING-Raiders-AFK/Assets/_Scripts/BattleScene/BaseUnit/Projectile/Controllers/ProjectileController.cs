using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private const float PROJECTILE_IMPACT_RADIUS = 0.5f;
    [SerializeField] private ProjectileView _view;
    
    private BaseUnitController _target;
    private ProjectileModel _model;
    
    private float _damage;
    
    private float _distance => Vector3.Distance(transform.position, _target.transform.position);
    
    public void Init(BaseUnitController target, ProjectileModel projectileModel, float damageValue)
    {
        _model = projectileModel;
        _target = target;
        _damage = damageValue;
        _view.Setup(_model);
    }

    public void Launch()
    {
        StartCoroutine(MoveTowardsPosition());
    }

    private IEnumerator MoveTowardsPosition()
    {
        while (_distance >= PROJECTILE_IMPACT_RADIUS)
        {
            var movePos = Vector3.MoveTowards(transform.position, _target.transform.position,
                _model.FlightSpeed * Time.deltaTime);

            transform.rotation = LookAtTarget(movePos - transform.position);
            transform.position = movePos;

            yield return null;
        }

        if (!_target.ActualStats.IsDead)
        {
            DoOnImpact();
        }

        Destroy(gameObject);
    }

    private static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void DoOnImpact()
    {
        _target.ChangeHealth(-_damage);

        if (_model.StatusEffect)
        {
            _target.AddStatusEffect(_model.StatusEffect);
        }
    }
}