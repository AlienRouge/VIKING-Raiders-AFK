using System;
using _Scripts.Enums;
using UnityEngine;

[Serializable]
public class ActualUnitStats
{
    [SerializeField] private BaseUnitModel _model;
    [SerializeField] private Team _battleTeam;
    [SerializeField] private int _unitLevel;

    [SerializeField] private float _health; //+
    [SerializeField] private float _damage; //+
    [SerializeField] private float _armour; //
    [SerializeField] private float _attackDeltaTime; //
    [SerializeField] private float _attackRange; //
    [SerializeField] private float _moveSpeed; //
    [SerializeField] private float _critChance; //

    private BaseUnitController _parent;
    public BaseUnitModel Model => _model;
    public Team BattleTeam => _battleTeam;
    public int UnitLevel => _unitLevel;
    public bool IsDead => _health <= 0;

    public ActualUnitStats(BaseUnitModel model, Team unitTeam, int unitLevel, BaseUnitController parent)
    {
        _model = model;
        _parent = parent;
        _battleTeam = unitTeam;
        _unitLevel = unitLevel;
        _health = model.BaseHealth;
        _damage = _model.GetUnitDamage(_unitLevel);
        _armour = _model.GetUnitArmour(_unitLevel);
        _attackDeltaTime = _model.AttackDeltaTime;
        _attackRange = _model.AttackRange;
        _moveSpeed = _model.MoveSpeed;
        _critChance = _model.CritChance;
    }

    public float Health
    {
        get => _health;
        set { _health = value; }
    }

    public float Damage
    {
        get => _damage;
        set { _damage = value; }
    }

    public float Armour
    {
        get => _armour;
        set { _armour = value; }
    }

    public float AttackDeltaTime
    {
        get => _attackDeltaTime;
        set { _attackDeltaTime = value; }
    }

    public float AttackRange
    {
        get => _attackRange;
        set { _attackRange = value; }
    }
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            _moveSpeed = value;
            _parent.ChangeMoveSpeed(_moveSpeed);
        }
    }

    public float CritChance
    {
        get => _critChance;
        set { _critChance = value; }
    }
    
    public void RestoreDamageValue()
    {
        Damage = _model.GetUnitDamage(_unitLevel);
    }
    public void RestoreMoveSpeedValue()
    {
        MoveSpeed = _model.MoveSpeed;
    }
}