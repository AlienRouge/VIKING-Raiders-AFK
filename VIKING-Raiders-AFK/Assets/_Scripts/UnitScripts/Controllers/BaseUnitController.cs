using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseUnitController : MonoBehaviour
{
    private MovementController _movementController;
    private BaseUnitView _baseUnitView;
    private DragContoller _dragContoller;

    private BaseUnitModel _model;
    private BaseUnitController _currentTarget;

    public Team MyTeam { get; private set; }

    private float _health;
    private float _attackDeltaTime;
    private bool _isBattleEnd;
    private bool isDead => _health <= 0;


    private bool isTargetInRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= attackRange;

    public string characterName => _model.characterName;
    private float attackRange => _model.attackRange;

    public UnityAction<Team, BaseUnitController> UnitDead;

    public void StartBattle()
    {
        _movementController.Enable();
        _dragContoller.Disable();

        FindTarget();
        StartBattleCycle();
    }

    public void Init(BaseUnitModel model, Team team, bool isDraggable)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();
        _dragContoller = GetComponent<DragContoller>();

        _model = model;
        _health = model.baseHealth;
        _attackDeltaTime = 1 / model.attackSpeed;
        MyTeam = team;


        _movementController.Init(model.moveSpeed);
        _baseUnitView.Init(_model);

        if (isDraggable)
            _dragContoller.Enable();
        else
            _dragContoller.Disable();
        
        // ability?.Init();
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range visualization on select
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnEnable()
    {
        EventController.BattleEnded += OnBattleEnded;
    }

    private void OnDisable()
    {
        EventController.BattleEnded -= OnBattleEnded;
    }

    private void OnBattleEnded()
    {
        _isBattleEnd = true;
    }

    private void FindTarget()
    {
        var enemies = BattleController.instance.GetEnemies(MyTeam);
        float minDistance = Mathf.Infinity;
        BaseUnitController supposedEnemy = null;
        _currentTarget = null;

        // TODO Find with priority

        foreach (var enemy in enemies)
        {
            float distance = _movementController.CalculatePathLength(enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                supposedEnemy = enemy;
            }
        }

        _currentTarget = supposedEnemy;

        if (_currentTarget)
        {
            _currentTarget.UnitDead += OnTargetDeadHandler;
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{characterName}: Found new target({_currentTarget.characterName})."
            : $"{characterName}: No targets.");
    }

    private void OnTargetDeadHandler(Team team, BaseUnitController unit)
    {
        FindTarget();
    }

    private async void StartBattleCycle()
    {
        while (!isDead && !_isBattleEnd)
        {
            await Task.Yield();
            if (FollowTarget())
                continue;

            await Attack();
        }
    }

    private bool FollowTarget()
    {
        if (isDead) return false;

        if (!_isBattleEnd && !isTargetInRange)
        {
            _movementController.Resume();
            return true;
        }

        _movementController.Stop();
        return false;
    }

    private async Task Attack()
    {
        if (isDead || _isBattleEnd || _currentTarget.isDead) return;
        _currentTarget.TakeDamage(_model.baseDamage);
        await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
    }

    private void TakeDamage(float dmg)
    {
        if (dmg < 0)
            throw new ArgumentOutOfRangeException(nameof(dmg));

        Debug.Log($"{_model.characterName}: Taking damage: {dmg}dmg");
        _health -= dmg;

        _baseUnitView.OnTakeDamage(_health);
        if (isDead)
        {
            Debug.Log($"{_model.characterName} dead.");
            /*EventController.UnitDied?.Invoke(_myTeam, this);*/
            _currentTarget = null;
            UnitDead.Invoke(MyTeam, this);
        }
    }
}