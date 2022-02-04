using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

public abstract class BaseUnitModel : ScriptableObject
{
    [SerializeField] public string CharacterName;
    [SerializeField] protected string description;

    [SerializeField, Header("View sprite")]
    public Sprite viewSprite;

    [SerializeField] public Vector3 spriteScale = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField, Header("Attributes")] public float BaseDamage;
    [SerializeField] public float BaseHealth = 1.0f;
    [SerializeField, Range(1, 5)] public float AttackRange = 1.0f;
    [SerializeField] protected float armor;
    [SerializeField] protected UnitType unitType;
    [Header("Speed")] [SerializeField] public float MoveSpeed;
    [SerializeField, Range(1, 5)] public float AttackSpeed = 1.0f;

    [Header("Multiplier")] [SerializeField, Range(0.5f, 3f)]
    protected float damageMultiplier;

    [SerializeField] protected PhysicalDamageType physicalDamageType;
}