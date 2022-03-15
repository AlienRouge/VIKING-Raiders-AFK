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
    private BaseUnitView _baseUnitView;

    [field: SerializeField] public Team MyTeam { get; private set; }
    [SerializeField] private int _level = 1;
    [SerializeField] private float _health;
    
    // Debug 
    [SerializeField] private float Damage;
    [SerializeField] private float Armour;
    // EndDebug
    private float _attackDeltaTime;
    private bool _isBattleEnd;
    private bool isDead => _health <= 0;

    private bool isTargetInRange =>
        Vector3.Distance(transform.position, _currentTarget.transform.position) <= attackRange;

    public string characterName => _model.CharacterName;
    private float attackRange => _model.AttackRange;

    public UnityAction<Team, BaseUnitController> UnitDead;

    private MovementController _movementController;
    private DragDropController _dragDropController;

    public void StartBattle()
    {
        Damage = GetDamageValue();
        Armour = GetArmourValue();
        
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
        
        _currentTarget.TakeDamage(CalculateDamage());
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
        // var critRatio = Random.Range(0f, 1f) < _model.CriticalRate;
        var attackRatio = dmgRatio - lvlRatio;
        var finalDmg = (int) Mathf.Floor(GetDamageValue() * attackRatio);

        return finalDmg > 0 ? finalDmg : 0;
    }

    private void TakeDamage(float dmg)
    {
        if (dmg < 0)
            throw new ArgumentOutOfRangeException(nameof(dmg));

        Debug.Log($"{_model.CharacterName}: Taking damage: {dmg}dmg");
        _health -= dmg;

        _baseUnitView.OnTakeDamage(_health);
        if (isDead)
        {
            Debug.Log($"{_model.CharacterName} dead.");
            /*EventController.UnitDied?.Invoke(_myTeam, this);*/
            _currentTarget = null;
            UnitDead.Invoke(MyTeam, this);
        }
    }
}