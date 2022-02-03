using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

public abstract class BaseUnitModel : ScriptableObject
{
    [SerializeField] protected new string name;
    [SerializeField] protected string description;
    [SerializeField, Header("Put link on view game object")] protected GameObject viewGameObject;
    [SerializeField,Space] protected float baseDamage;
    [SerializeField] protected float baseHealth;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float armor;
    [SerializeField] protected UnitType unitType;
    [Header("Speed")]
    [SerializeField] protected float moveSpeed;
    [SerializeField, Range(1, 5)] protected float attackSpeed;
    [Header("Multiplier")]
    [SerializeField, Range(0.5f, 3f)] protected float damageMultiplier;
    
    [SerializeField] protected PhysicalDamageType physicalDamageType;
}
