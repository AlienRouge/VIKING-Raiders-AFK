using System;
using System.Threading.Tasks;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseUnitController : MonoBehaviourPun
{
    private BaseUnitController _currentTarget;
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
        ActualStats = new ActualUnitStats(model, team, unitLevel);
    }

    private void InitializeControllers(bool isDraggable)
    {
        _movementController = GetComponent<MovementController>();
        _baseUnitView = GetComponentInChildren<BaseUnitView>();
        _dragDropController = GetComponent<DragDropController>();
        _abilityController = GetComponent<AbilityController>();
        _statusEffectController = GetComponent<StatusEffectController>();

        _movementController.Init(ActualStats.UnitModel.MoveSpeed);
        _baseUnitView.Init(this);
        _dragDropController.Init(ActualStats.BattleTeam, isDraggable);
        _statusEffectController.Init();

        if (ActualStats.UnitModel.PassiveAbility)
        {
            EventController.PassiveAbilityStateChanged += OnPassiveAbilityStateChange;
            _abilityController.InitPassiveAbility(ActualStats.UnitModel.PassiveAbility);
        }

        if (ActualStats.UnitModel.ActiveAbility)
        {
            EventController.UseUnitActiveAbility += OnUseActiveAbility;
            _abilityController.InitActiveAbility(ActualStats.UnitModel.ActiveAbility);
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
        _currentTarget = BattleController.instance.GetTarget(this);

        if (_currentTarget)
        {
            _movementController.SetTarget(_currentTarget);
        }

        Debug.Log(_currentTarget
            ? $"{ActualStats.UnitModel.CharacterName}: New target({_currentTarget.ActualStats.UnitModel.CharacterName})."
            : $"{ActualStats.UnitModel.CharacterName}: No targets.");
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
            $"{ActualStats.UnitModel.CharacterName} --> {_currentTarget.ActualStats.UnitModel.CharacterName} [{damage}]dmg");
        _currentTarget.ChangeHealth(-damage);
        await Task.Delay(Mathf.RoundToInt(ActualStats.AttackDeltaTime * Consts.ONE_SECOND_VALUE));
    }

    private int CalculateDamage()
    {
        var dmgRatio = ActualStats.GetDamageValue() / _currentTarget.ActualStats.GetArmourValue();
        var lvlRatio = (_currentTarget.ActualStats.UnitLevel - ActualStats.UnitLevel) * 0.05f; // Value per level
        var critRatio = Random.Range(0f, 1f) < ActualStats.UnitModel.CriticalRate; // TODO Рейт в актуальные?
        var attackRatio = dmgRatio - lvlRatio + Convert.ToInt32(critRatio);
        var finalDmg =
            (int)Mathf.Floor(ActualStats.DmgMultiplier * ActualStats.GetDamageValue() * attackRatio *
                             Random.Range(0.85f, 1f));

        if (critRatio)
        {
            Debug.Log($"{ActualStats.UnitModel.CharacterName}: CRITICAL DAMAGE");
        }

        return finalDmg > 0 ? finalDmg : 0;
    }
    private async Task UsePassiveAbility()
    {
        if (ActualStats.IsDead || _isBattleEnd || _currentTarget.ActualStats.IsDead) return;

        await _abilityController.ActivatePassiveAbility(_currentTarget);
    }

    private async void UseActiveAbility()
    {
        if (!ActualStats.UnitModel.ActiveAbility) return;

        _isCasting = true;
        CurrentMoveState = MoveState.Stop;
        await _abilityController.ActivateActiveAbility(_currentTarget);
        _isCasting = false;
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
        ActualStats.CurrentHealth += amount;
        ActualStats.CurrentHealth = Mathf.Clamp(ActualStats.CurrentHealth, 0, ActualStats.UnitModel.BaseHealth);
        EventController.UnitHealthChanged.Invoke(this, ActualStats.CurrentHealth);

        if (ActualStats.IsDead) 
        {
            OnDeathHandler();
        }
    }

    private void OnDeathHandler()
    {
        _currentTarget = null;
        if (ActualStats.UnitModel.PassiveAbility)
        {
            EventController.PassiveAbilityStateChanged -= OnPassiveAbilityStateChange;
        }
        if (ActualStats.UnitModel.ActiveAbility)
        {
            EventController.UseUnitActiveAbility -= OnUseActiveAbility;
        }
        
        _statusEffectController.OnUnitDead();
        BattleController.instance.OnUnitDied(this);
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

        if (ActualStats.UnitModel.PassiveAbility)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ActualStats.UnitModel.PassiveAbility.CastRange);
        }

        if (ActualStats.UnitModel.ActiveAbility)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ActualStats.UnitModel.ActiveAbility.CastRange);
        }
    }
}