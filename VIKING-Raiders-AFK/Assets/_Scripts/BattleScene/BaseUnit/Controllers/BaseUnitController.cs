using System;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BaseUnitController : MonoBehaviour
{
    private BaseUnitModel _model;
    private BaseUnitController _currentTarget;

    [field: SerializeField] public Team MyTeam { get; private set; }

    [SerializeField] private int _level;
    [SerializeField] private float _currentHealth;

    [SerializeField] private MoveState _currentMoveState;
    
    private float _attackDeltaTime;
    private bool _isBattleEnd;
    public bool isDead => _currentHealth <= 0;
    public string characterName => _model.CharacterName;

    private float _attackRange => _model.AttackRange;
    private float _abilityRange => _model.Ability.CastRange;

    private MoveState CurrentMoveState
    {
        get => _currentMoveState;
        set
        {
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
        get => _isTargetInRange;
        set
        {
            if (_isTargetInRange == value) return;
            var newMoveState = CurrentMoveState == MoveState.Move ? MoveState.Stop : MoveState.Move;
            CurrentMoveState = newMoveState;
            _isTargetInRange = value;
        }
    }

    private bool CheckAttackRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= _attackRange;

    // private bool CheckAbilityRange =>
    //     Vector3.Distance(transform.position, _currentTarget.transform.position) <= _abilityRange;


    [SerializeField] private bool _isAbilityReady;

    private enum MoveState
    {
        Move,
        Stop
    }

    private BaseUnitView _baseUnitView;
    private MovementController _movementController;
    private DragDropController _dragDropController;
    private AbilityController _abilityController;

    public void StartBattle()
    {
        _movementController.Enable();
        _dragDropController.enabled = false;
        EventController.UnitDied += OnTargetDeadHandler;

        FindTarget();
        StartBattleCycle();
    }

    public void Init(BaseUnitModel model, Team team, int unitLevel, bool isDraggable)
    {
        InitializeData(model, team, unitLevel);
        InitializeControllers(isDraggable);
    }

    private void InitializeData(BaseUnitModel model, Team team, int unitLevel)
    {
        _model = model;
        _level = unitLevel;
        _currentHealth = model.BaseHealth;
        _attackDeltaTime = 1 / model.AttackSpeed;
        MyTeam = team;
    }

    private void InitializeControllers(bool isDraggable)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();
        _dragDropController = GetComponent<DragDropController>();
        _abilityController = GetComponent<AbilityController>();

        _movementController.Init(_model.MoveSpeed);
        _baseUnitView.Init(_model);
        _dragDropController.Init(MyTeam, isDraggable);

        if (_model.Ability)
        {
            _isAbilityReady = true;
            _abilityController.AbilityReady += () =>
            {
                _isAbilityReady = true; // TODO Спрятать все сеттеры 
            };
            _abilityController.Init(_model.Ability);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range visualization on inspector select
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        if (!_model.Ability) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _abilityRange);
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
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{characterName}: New target({_currentTarget.characterName})."
            : $"{characterName}: No targets.");
    }

    private void OnTargetDeadHandler(BaseUnitController unit)
    {
        if (unit == _currentTarget)
        {
            FindTarget();
        }
    }

    private async void StartBattleCycle()
    {
        while (!isDead && !_isBattleEnd)
        {
            if (_isAbilityReady)
            {
                TargetInRange = _abilityController.CheckAbilityRange(this, _currentTarget);
                if (TargetInRange)
                {
                    await UseAbility();
                    continue;
                }
            }

            TargetInRange = CheckAttackRange; //? 
            if (TargetInRange)
            {
                await Attack();
            }

            await Task.Yield();
        }
    }

    private async Task Attack()
    {
        if (isDead || _isBattleEnd || _currentTarget.isDead) return;

        var damage = CalculateDamage();
        Debug.Log($"{_model.CharacterName} --> {_currentTarget.characterName} [{damage}]dmg");
        _currentTarget.ChangeHealth(-damage);
        await Task.Delay(Mathf.RoundToInt(_attackDeltaTime * 1000));
    }

    private async Task UseAbility()
    {
        if (isDead || _isBattleEnd || _currentTarget.isDead) return;

        await _abilityController.ActivateAbility(this, _currentTarget);
        _isAbilityReady = false;
    }

    public void ChangeHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _model.BaseHealth);

        _baseUnitView.OnChangeHealth(_currentHealth);
        if (isDead)
        {
            _currentTarget = null;
            EventController.UnitDied?.Invoke(this);
        }
    }
    // OnHealthChanged?.Invoke();

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
}