using System;
using System.Threading.Tasks;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BaseUnitController : MonoBehaviourPun
{
    // TODO Сортировка полей и методов
    private BaseUnitModel _model;
    private BaseUnitController _currentTarget;

    [field: SerializeField] public Team MyTeam { get; protected set; }

    private int _level;
    [SerializeField] private float _currentHealth;

    [SerializeField] private MoveState _currentMoveState;

    private float _attackDeltaTime;
    private bool _isBattleEnd;
    private bool isCastingActiveAbility;
    public bool isDead => _currentHealth <= 0;
    public string characterName => _model.CharacterName;

    private float _attackRange => _model.AttackRange;

    private MoveState CurrentMoveState
    {
        get => _currentMoveState;
        set
        {
            OnCurrentMoveStateSet(value);
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
            OnTargetInRangeSet();
            _isTargetInRange = value;
        }
    }

    public float CurrentHealth
    {
        get => _currentHealth;
        set {}
    }

    private bool CheckAttackRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= _attackRange;

    private bool _isPassiveAbilityReady;
    private bool _isActiveAbilityReady;

    private enum MoveState
    {
        Move,
        Stop
    }

    private BaseUnitView _baseUnitView;
    private MovementController _movementController;
    private DragDropController _dragDropController;
    private AbilityController _abilityController;
    private StatusEffectController _statusEffectController;

    public void StartBattle()
    {
        PreFightSetup();
        FindTarget();
        StartBattleCycle();
    }

    private void PreFightSetup()
    {
        _movementController.Enable();
        _dragDropController.enabled = false;
    }

    public void Init(BaseUnitModel model, Team team, int unitLevel, bool isDraggable)
    {
        InitializeData(model, team, unitLevel);
        InitializeControllers(isDraggable);
    }

    public BaseUnitModel GetUnitModel()
    {
        return _model;
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
        _statusEffectController = GetComponent<StatusEffectController>();
        
        _movementController.Init(_model.MoveSpeed);
        _baseUnitView.Init(_model);
        _dragDropController.Init(MyTeam, isDraggable);
        _statusEffectController.Init();

        if (_model.PassiveAbility)
        {
            EventController.PassiveAbilityStateChanged += OnPassiveAbilityStateChange;
            _abilityController.InitPassiveAbility(_model.PassiveAbility);
        }

        if (_model.ActiveAbility)
        {
            EventController.UseUnitActiveAbility += OnUseActiveAbility;
            _abilityController.InitActiveAbility(_model.ActiveAbility);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range visualization on inspector select
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        if (_model.PassiveAbility)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _model.PassiveAbility.CastRange);
        }

        if (_model.ActiveAbility)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _model.ActiveAbility.CastRange);
        }
    }
    
    private void OnEnable()
    {
        EventController.BattleEnded += OnBattleEnded;
        EventController.UnitDied += OnTargetDeathHandler;
    }

    private void OnDisable()
    {
        EventController.BattleEnded -= OnBattleEnded;
        EventController.UnitDied -= OnTargetDeathHandler;
    }

    private void OnBattleEnded()
    {
        _isBattleEnd = true;
        CurrentMoveState = MoveState.Stop;
    }

    private void FindTarget()
    {
        _currentTarget = BattleController.instance.GetTarget(this);

        if (_currentTarget)
        {
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{characterName}: New target({_currentTarget.characterName})."
            : $"{characterName}: No targets.");
    }

    public float GetDistanceToPosition(Vector3 position)
    {
        return _movementController.CalculatePathLength(position);
    }

    public void AddStatusEffect(BaseStatusEffect effect)
    {
        _statusEffectController.AddStatusEffect(effect);
    }

    private void OnTargetDeathHandler(BaseUnitController unit)
    {
        Debug.Log("DEAD: "+unit);
        if (unit == _currentTarget)
        {
            Debug.Log("NO problem find new one");
            FindTarget();
        }
    }

    private async void StartBattleCycle()
    {
        while (!isDead && !_isBattleEnd)
        {
            if (!isCastingActiveAbility)
            {
                if (_isPassiveAbilityReady)
                {
                    TargetInRange = _abilityController.CheckPassiveAbilityRange(_currentTarget);
                    if (TargetInRange)
                    {
                        await UsePassiveAbility();
                        continue;
                    }
                }

                TargetInRange = CheckAttackRange; //? 
                if (TargetInRange)
                {
                    await Attack();
                    continue;
                }

                CurrentMoveState = MoveState.Move;
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

    private async Task UsePassiveAbility()
    {
        if (isDead || _isBattleEnd || _currentTarget.isDead) return;

        await _abilityController.ActivatePassiveAbility(_currentTarget);
    }

    private async void UseActiveAbility()
    {
        if (!_model.ActiveAbility) return;

        isCastingActiveAbility = true;
        CurrentMoveState = MoveState.Stop;
        await _abilityController.ActivateActiveAbility(_currentTarget);
        isCastingActiveAbility = false;
    }

    private void OnUseActiveAbility(BaseUnitController unit)
    {
        if (ReferenceEquals(unit, this))
        {
            UseActiveAbility();
        }
    }

    private void OnPassiveAbilityStateChange(BaseUnitController unit, AbilityController.AbilityState state)
    {
        Debug.Log(unit);
        if (!ReferenceEquals(unit, this)) return;
        
        _isPassiveAbilityReady = state == AbilityController.AbilityState.Ready;
    }

    private void OnCurrentMoveStateSet(MoveState value)
    {
        var moveFlag = value == MoveState.Stop;
        _movementController.IsStopped(moveFlag);
    }

    private void OnTargetInRangeSet()
    {
        var oppositeMoveState = CurrentMoveState == MoveState.Move ? MoveState.Stop : MoveState.Move;
        CurrentMoveState = oppositeMoveState;
    }

    public void ChangeHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _model.BaseHealth);

        _baseUnitView.OnChangeHealth(_currentHealth); // TODO To event
        if (isDead)
        {
            _currentTarget = null;
            
            if (_model.PassiveAbility)
            {
                EventController.PassiveAbilityStateChanged -= OnPassiveAbilityStateChange;

            }
            if (_model.ActiveAbility)
            {
                EventController.UseUnitActiveAbility -= OnUseActiveAbility;
            }
            
            _statusEffectController.OnUnitDead();
            BattleController.instance.OnUnitDied(this);
            EventController.UnitDied?.Invoke(this);
        }
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
}