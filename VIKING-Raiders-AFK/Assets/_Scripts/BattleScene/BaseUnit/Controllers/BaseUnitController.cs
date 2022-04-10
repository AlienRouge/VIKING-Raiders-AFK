using System;
using System.Threading.Tasks;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(StatusEffectController), typeof(AbilityController), typeof(DragDropController))]
[RequireComponent(typeof(BaseUnitView), typeof(MovementController))]
public abstract class BaseUnitController : MonoBehaviourPun
{
    protected BaseUnitController _currentTarget;
    private MoveState _currentMoveState;
    private bool _isPassiveAbilityReady;
    private bool _isActiveAbilityReady;
    private bool _isCasting;
    private bool _isTargetInRange;
    private bool _isBattleEnd;

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

    [field: SerializeField] public ActualUnitStats ActualStats { get; private set; }

    private MoveState CurrentMoveState
    {
        get => _currentMoveState;
        set
        {
            OnCurrentMoveStateSet(value);
            _currentMoveState = value;
        }
    }

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

    private bool TargetInAttackRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= ActualStats.AttackRange;


    public void Init(BaseUnitModel model, Team team, int unitLevel, bool isDraggable)
    {
        InitializeData(model, team, unitLevel);
        InitializeControllers(isDraggable);
    }

    private void InitializeData(BaseUnitModel model, Team team, int unitLevel)
    {
        ActualStats = new ActualUnitStats(model, team, unitLevel, this);
    }

    private void InitializeControllers(bool isDraggable)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();
        _dragDropController = GetComponent<DragDropController>();
        _abilityController = GetComponent<AbilityController>();
        _statusEffectController = GetComponent<StatusEffectController>();

        _movementController.Init(ActualStats.Model.MoveSpeed);
        _baseUnitView.Init(this);
        _dragDropController.Init(ActualStats.BattleTeam, isDraggable);
        _statusEffectController.Init();

        if (ActualStats.Model.PassiveAbility)
        {
            EventController.PassiveAbilityStateChanged += OnPassiveAbilityStateChange;
            _abilityController.InitPassiveAbility(ActualStats.Model.PassiveAbility);
        }

        if (ActualStats.Model.ActiveAbility)
        {
            EventController.UseUnitActiveAbility += OnUseActiveAbility;
            _abilityController.InitActiveAbility(ActualStats.Model.ActiveAbility);
        }
    }

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

    private void FindTarget()
    {
        _currentTarget = BattleSceneController.instance.BattleController.GetTarget(this);

        if (_currentTarget)
        {
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{ActualStats.Model.CharacterName}: New target({_currentTarget.ActualStats.Model.CharacterName})."
            : $"{ActualStats.Model.CharacterName}: No targets.");
    }

    private async void StartBattleCycle()
    {
        while (!ActualStats.IsDead && !_isBattleEnd)
        {
            if (!_isCasting)
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

                TargetInRange = TargetInAttackRange; //? 
                if (TargetInRange)
                {
                    await Attack();
                    continue;
                }

                CurrentMoveState = MoveState.Move;
            }

            await Task.Yield();
        }

        CurrentMoveState = MoveState.Stop;
    }

    private async Task Attack()
    {
        if (ActualStats.IsDead || _isBattleEnd || _currentTarget.ActualStats.IsDead) return;

        var damage = CalculateDamage();
        Debug.Log(
            $"{ActualStats.Model.CharacterName} --> {_currentTarget.ActualStats.Model.CharacterName} [{damage}]dmg");
        DoOnAttack(damage);

        await Task.Delay(Mathf.RoundToInt(ActualStats.AttackDeltaTime * Consts.ONE_SECOND_VALUE));
    }

    protected abstract void DoOnAttack(int damage);

    private int CalculateDamage()
    {
        // var dmgRatio = ActualStats.GetDamageValue() / _currentTarget.ActualStats.GetArmourValue();
        var armourRatio = 100.0f / (100.0f + _currentTarget.ActualStats.Armour);
        var perLevelRatio = (_currentTarget.ActualStats.UnitLevel - ActualStats.UnitLevel) * 0.05f; // Value per level
        var critRatio = Random.Range(0f, 1f) < ActualStats.Model.CritChance; // TODO Рейт в актуальные?
        var damageRatio = armourRatio - perLevelRatio + Convert.ToInt32(critRatio);
        var damage =
            (int)Mathf.Floor(ActualStats.Damage * damageRatio *
                             Random.Range(0.85f, 1.15f));

        if (critRatio)
        {
            Debug.Log($"{ActualStats.Model.CharacterName}: CRITICAL DAMAGE");
        }

        return damage > 0 ? damage : 0;
    }

    private async Task UsePassiveAbility()
    {
        if (ActualStats.IsDead || _isBattleEnd || _currentTarget.ActualStats.IsDead) return;

        await _abilityController.ActivatePassiveAbility(_currentTarget);
    }

    private async void UseActiveAbility()
    {
        if (!ActualStats.Model.ActiveAbility) return;

        _isCasting = true;
        CurrentMoveState = MoveState.Stop;
        await _abilityController.ActivateActiveAbility(_currentTarget);
        _isCasting = false;
        // CurrentMoveState = MoveState.Move;
    }

    public float GetDistanceToPosition(Vector3 position)
    {
        return _movementController.CalculatePathLength(position);
    }

    public void AddStatusEffect(BaseStatusEffect effect)
    {
        _statusEffectController.AddStatusEffect(effect);
    }

    public void ChangeHealth(float amount)
    {
        ActualStats.Health += amount;
        ActualStats.Health = Mathf.Clamp(ActualStats.Health, 0, ActualStats.Model.BaseHealth);
        EventController.UnitHealthChanged.Invoke(this, ActualStats.Health);
        
        if (ActualStats.IsDead)
        {
            OnDeathHandler();
        }
    }

    public void ChangeMoveSpeed(float speed)
    {
        _movementController.SetMoveSpeed(speed);
    }

    private void OnDeathHandler()
    {
        _currentTarget = null;

        if (ActualStats.Model.PassiveAbility)
        {
            EventController.PassiveAbilityStateChanged -= OnPassiveAbilityStateChange;
        }

        if (ActualStats.Model.ActiveAbility)
        {
            EventController.UseUnitActiveAbility -= OnUseActiveAbility;
        }

        _statusEffectController.OnUnitDead();
        BattleSceneController.instance.BattleController.OnUnitDied(this);
        EventController.UnitDied?.Invoke(this);
    }

    private void OnBattleEnded()
    {
        _isBattleEnd = true;
    }

    private void OnTargetDeath(BaseUnitController unit)
    {
        Debug.Log("DEAD: " + unit);
        if (unit == _currentTarget)
        {
            FindTarget();
        }
    }

    private void OnTargetInRangeSet()
    {
        var oppositeMoveState = CurrentMoveState == MoveState.Move ? MoveState.Stop : MoveState.Move;
        CurrentMoveState = oppositeMoveState;
    }

    private void OnCurrentMoveStateSet(MoveState value)
    {
        var moveFlag = value == MoveState.Stop;
        _movementController.IsStopped(moveFlag);
    }

    private void OnPassiveAbilityStateChange(BaseUnitController unit, AbilityController.AbilityState state)
    {
        if (!ReferenceEquals(unit, this)) return;

        _isPassiveAbilityReady = state == AbilityController.AbilityState.Ready;
    }

    private void OnUseActiveAbility(BaseUnitController unit)
    {
        if (ReferenceEquals(unit, this))
        {
            UseActiveAbility();
        }
    }

    private void OnEnable()
    {
        EventController.BattleEnded += OnBattleEnded;
        EventController.UnitDied += OnTargetDeath;
    }

    private void OnDisable()
    {
        EventController.BattleEnded -= OnBattleEnded;
        EventController.UnitDied -= OnTargetDeath;
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range visualization on inspector select
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ActualStats.AttackRange);

        if (ActualStats.Model.PassiveAbility)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ActualStats.Model.PassiveAbility.CastRange);
        }

        if (ActualStats.Model.ActiveAbility)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ActualStats.Model.ActiveAbility.CastRange);
        }
    }
}