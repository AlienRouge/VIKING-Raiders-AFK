using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Interfaces;
using UnityEngine;

public abstract class BaseUnitModelModel : ScriptableObject, IUnitModel
{
    [Header("UnitDescription")]
    [SerializeField] protected string _characterName;
    public string characterName => _characterName;
    [SerializeField] protected string _description;
    public string description => _description;
    [SerializeField, Header("View sprite")] protected Sprite _viewSprite;
    public Sprite viewSprite => _viewSprite;
    [SerializeField] protected Vector3 _spriteScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 spriteScale => _spriteScale;
    
    [Header("Attributes")]
    [SerializeField] private float _baseDamage;
    public float baseDamage => _baseDamage;
    [SerializeField] protected float _baseHeath;
    public float baseHealth => _baseHeath;

    [SerializeField, Range(1, 5)] protected float _attackRange = 1.0f;
    public float attackRange => _attackRange;
    [SerializeField] protected float _armor;
    public float armor => _armor;
    [SerializeField] protected UnitType _unitType;
    public UnitType unitType => _unitType;
    
    [Header("Speed")]
    [SerializeField] protected float _moveSpeed;
    public float moveSpeed => _moveSpeed;
    [SerializeField, Range(1, 5)] protected float _attackSpeed = 1.0f;
    public float attackSpeed => _attackSpeed;
    
    [Header("Multiplier")] 
    [SerializeField, Range(0.5f, 3f)] protected float _damageMultiplier;
    public float damageMultiplier => _damageMultiplier;

    [SerializeField] protected Ability _ability;
    public Ability ability => _ability;

}