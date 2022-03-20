using System;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BaseUnitController : MonoBehaviour
{
    private BaseUnitModel _model;
    private BaseUnitController _currentTarget;
    
    [field: SerializeField] public Team MyTeam { get; private set; }
    
    [SerializeField] private int _level;
    [SerializeField] private float _health;
    [SerializeField] private MoveState _currentMoveState;
    public UnityAction<Team, BaseUnitController> UnitDead;
    private float _attackDeltaTime;
    private bool _isBattleEnd;
    private bool isDead => _health <= 0;
    public string characterName => _model.CharacterName;

    private float attackRange => _model.AttackRange;
    
    private MoveState CurrentMoveState
    {
        get => _currentMoveState;
        set
        {
            Debug.Log(characterName);
            if (value == MoveState.Move)
            {
                _movementController.Resume();
            }
            else
            {
                _movementController.Stop();
            }

            _currentMoveState = value;
        }
    }
    
    private bool _isTargetInRange;
    private bool TargetInRange
    {
        set
        {
            if (_isTargetInRange == value) return;
            var newMoveState = CurrentMoveState == MoveState.Move ? MoveState.Stop : MoveState.Move;
            CurrentMoveState = newMoveState;
            _isTargetInRange = value;
        }
    }

    private bool CheckAttackRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= attackRange;

    // private bool isTargetInAbilityRange =>
    //     Vector3.Distance(transform.position, _currentTarget.transform.position) <= abilityRange;


    
    // private float abilityRange => _model.Ability.CastRange;

    private enum MoveState
    {
        Move,
        Stop
    }
    
    private BaseUnitView _baseUnitView;
    private MovementController _movementController;
    private DragDropController _dragDropController;

    public void StartBattle()
    {
        _movementController.Enable();
        _dragDropController.enabled = false;

        FindTarget();
        StartBattleCycle();
    }

    public void Init(BaseUnitModel model, Team team, int unitLevel, bool isDraggable)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();
        _dragDropController = GetComponent<DragDropController>();

        _model = model;
        _level = unitLevel;
        _health = model.BaseHealth;
        _attackDeltaTime = 1 / model.AttackSpeed;
        MyTeam = team;


        _movementController.Init(model.MoveSpeed);
        _baseUnitView.Init(_model);
        _dragDropController.Init(MyTeam);

        _dragDropController.enabled = isDraggable;
        // ability?.Init();
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range visualization on inspector select
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
        CurrentMoveState = MoveState.Stop;
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
            ? $"{characterName}: New target({_currentTarget.characterName})."
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
            FollowTarget();

            if (CheckAttackRange)
            {
                await Attack();
            }
            
            await Task.Yield();
        }
    }

    private void FollowTarget()
    {
        TargetInRange = CheckAttackRange;
    }

    private async Task Attack()
    {
        // if (isDead || _isBattleEnd || _currentTarget.isDead) return;

        var damage = CalculateDamage();
        Debug.Log($"{_model.CharacterName} --> {_currentTarget.characterName} [{damage}]dmg");
        _currentTarget.TakeDamage(damage);
        await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
    }

    private float GetArmourValue()
    {
        return _model.GetArmourPerUnitLevel(_level);
    }

    private float GetDamageValue()
    {
        return _model.GetDamagePerUnitLevel(_level);
    }

    private int CalculateDamage()
    {
        var dmgRatio = GetDamageValue() / _currentTarget.GetArmourValue();
        var lvlRatio = (_currentTarget._level - _level) * 0.05f; // Value per level
        var critRatio = Random.Range(0f, 1f) < _model.CriticalRate;
        var attackRatio = dmgRatio - lvlRatio + Convert.ToInt32(critRatio);
        var finalDmg = (int)Mathf.Floor(GetDamageValue() * attackRatio * Random.Range(0.85f, 1f));

        if (critRatio)
        {
            Debug.Log($"{_model.CharacterName}: CRITICAL DAMAGE");
        }

        return finalDmg > 0 ? finalDmg : 0;
    }

    private void TakeDamage(float dmg)
    {
        if (dmg < 0)
            throw new ArgumentOutOfRangeException(nameof(dmg));

        _health -= dmg;
        _baseUnitView.OnTakeDamage(_health);
        if (isDead)
        {
            /*EventController.UnitDied?.Invoke(_myTeam, this);*/
            _currentTarget = null;
            UnitDead.Invoke(MyTeam, this);
        }
    }
}