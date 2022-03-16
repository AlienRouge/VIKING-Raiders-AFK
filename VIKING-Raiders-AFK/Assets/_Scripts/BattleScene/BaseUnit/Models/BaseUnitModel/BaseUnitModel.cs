using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

public abstract class BaseUnitModel : ScriptableObject
{
    [field: Header("UnitDescription")]
    [field: SerializeField]
    public string CharacterName { get; private set; }

    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField, Header("View Sprite")]
    public Sprite ViewSprite { get; private set; }

    [field: SerializeField] public Vector3 ViewSpriteScale { get; private set; }

    [field: Header("Attributes")]
    [field: SerializeField]
    public UnitType UnitType { get; private set; }
    [field: SerializeField] public int MaxUnitLevel { get; private set; }
    [field: SerializeField] public int BaseHealth { get; private set; }
    [field: SerializeField] public int BaseDamage { get; private set; }
    [field: SerializeField] public int BaseArmour { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float CriticalRate { get; private set; }
    
    [field: SerializeField, Range(1, 7)] public float AttackRange { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public Ability Ability { get; private set; }

    public float GetDamagePerUnitLevel(int unitLevel)
    {
        return BaseDamage * (1 + unitLevel / MaxUnitLevel);
    }

    public float GetArmourPerUnitLevel(int unitLevel)
    {
        return BaseArmour * (1 + unitLevel / MaxUnitLevel);
    }
}