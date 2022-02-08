using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

public class BaseUnitController : MonoBehaviour
{
    private MovementController _movementController;
    private BaseUnitView _baseUnitView;

    private BaseUnitModel _model;
    private BaseUnitController _currentTarget;

    private Team _myTeam;
    private float _health;
    private float _attackDeltaTime;
    private bool _isDead => _health <= 0;

    private bool _targetInRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= _attackRange;

    private string _characterName => _model.characterName;
    private float _attackRange => _model.attackRange;

    private UnityAction _unitDead;


    void OnDrawGizmosSelected()
    {
        // Attack range visualization on select
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private void Start()
    {
        FindTarget();
        StartBattle();
    }

    public void Init(BaseUnitModel model, Team team, Vector3 startPosition)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();

        _model = model;
        _health = model.baseHealth;
        _attackDeltaTime = 1 / model.attackSpeed;
        _myTeam = team;

        _movementController.Init(model.moveSpeed, model.attackRange);
        _baseUnitView.Init(_model);
        // ability?.Init();


        transform.position = startPosition; // ?
    }

    private void FindTarget()
    {
        var enemies = BattleController.Instance.GetEnemies(_myTeam);
        float minDistance = Mathf.Infinity;
        BaseUnitController supposedEnemy = null;
        _currentTarget = null;

        // TODO Find with priority
        foreach (var enemy in enemies.Where(enemy =>
                     Vector3.Distance(enemy.transform.position, this.transform.position) <= minDistance))
        {
            minDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
            supposedEnemy = enemy;
        }

        _currentTarget = supposedEnemy;

        if (_currentTarget)
        {
            _currentTarget._unitDead += OnTargetDeadHandler;
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{_characterName}: Found new target({_currentTarget._characterName})."
            : $"{_characterName}: No targets.");
    }

    private void OnTargetDeadHandler()
    {
        FindTarget();
    }

    private async void StartBattle()
    {
        while (_currentTarget)
        {
            await Task.Yield();
            if (FollowTarget())
                continue;

            await Attack();
        }
    }

    private bool FollowTarget()
    {
        if (!_targetInRange)
        {
            _movementController.Resume();
            return true;
        }

        _movementController.Stop();
        return false;
    }

    private async Task Attack()
    {
        await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
        _currentTarget.TakeDamage(_model.baseDamage);
    }

    private void TakeDamage(float dmg)
    {
        if (dmg < 0)
            throw new ArgumentOutOfRangeException(nameof(dmg));

        Debug.Log($"{_model.characterName}: Taking damage: {dmg}dmg");
        _health -= dmg;

        _baseUnitView.OnTakeDamage(_health);
        if (_isDead)
        {
            Debug.Log($"{_model.characterName} dead.");
            EventController.UnitDied?.Invoke(_myTeam, this);
            _unitDead.Invoke();
        }
    }
}