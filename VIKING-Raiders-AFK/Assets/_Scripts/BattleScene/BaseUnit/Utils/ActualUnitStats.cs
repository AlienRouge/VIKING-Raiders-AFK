using System;
using _Scripts.Enums;
using UnityEngine;

[Serializable]
public class ActualUnitStats
{
    [SerializeField] private BaseUnitModel _unitModel;
    [SerializeField] private Team _battleTeam;
    [SerializeField] private int _unitLevel;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _attackDeltaTime;
    [SerializeField] private float _attackRange;
    
    [SerializeField] private float _dmgMultiplier;
    [SerializeField] private float _attackSpeedMultiplier;
    [SerializeField] private float _moveSpeedMultiplier;
    [SerializeField] private float _critRateMultiplier;
    
    public BaseUnitModel UnitModel => _unitModel;
    public Team BattleTeam => _battleTeam;
    public int UnitLevel => _unitLevel;
    public bool IsDead => _currentHealth <= 0;

    public float CurrentHealth
    {
        get => _currentHealth;
        set { _currentHealth = value; }
    }

    public float DmgMultiplier
    {
        get => _dmgMultiplier;
        set { _dmgMultiplier = value; }
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
    public ActualUnitStats(BaseUnitModel unitModel, Team unitTeam, int unitLevel)
    {
        _unitModel = unitModel;
        _battleTeam = unitTeam;
        _unitLevel = unitLevel;
        _currentHealth = unitModel.BaseHealth;
        _dmgMultiplier = 1.0f; // TODO
        _attackDeltaTime = 1.0f / unitModel.AttackSpeed;
        _attackRange = unitModel.AttackRange;
    }

    public float GetArmourValue()
    {
        return UnitModel.GetArmourPerUnitLevel(UnitLevel);
    }

    public float GetDamageValue()
    {
        return UnitModel.GetDamagePerUnitLevel(UnitLevel);
    }
}
