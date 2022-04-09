using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileController : MonoBehaviour
{
    private BaseUnitController _target;
    private ProjectileModel _model;
    private float _damage;

    private float _distance => Vector3.Distance(transform.position, _target.transform.position);
   


    [SerializeField] private ProjectileView _view;

    public void Fire(ProjectileModel projectileModel, BaseUnitController target, float damage)
    {
        _model = projectileModel;
        _target = target;
        _damage = damage;

        _view.Setup(_model);
        
        StartCoroutine(MoveTowardsPosition());
    }

    private IEnumerator MoveTowardsPosition()
    {
        while (_distance >= 0.5f)
        {
            Vector3 mP = Vector3.MoveTowards(transform.position, _target.transform.position,
                _model.FlightSpeed * Time.deltaTime);

            transform.rotation = LookAtTarget(mP - transform.position);
            transform.position = mP;

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
        Debug.Log("Impact!!");

        _target.ChangeHealth(-_damage);

        if (_model.StatusEffect)
        {
            _target.AddStatusEffect(_model.StatusEffect);
        }
    }
}