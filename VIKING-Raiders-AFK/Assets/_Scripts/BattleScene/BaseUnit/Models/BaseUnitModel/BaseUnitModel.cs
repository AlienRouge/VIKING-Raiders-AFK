using System;
using _Scripts.Enums;
using UnityEngine;

[Serializable]
public struct TargetWeights
{
    public float hpWeight;
    public float distanceWeight;
}

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
    [field: SerializeField, Range(0, 1)] public float CriticalRate { get; private set; }
    [field: SerializeField, Range(1, 7)] public float AttackRange { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public BaseAbility PassiveAbility { get; private set; }
    [field: SerializeField] public BaseAbility ActiveAbility { get; private set; }

    [SerializeField] private TargetWeights _targetWeights;

    public TargetWeights TargetWeights
    {
        get
        {
            if (_targetWeights.hpWeight + _targetWeights.distanceWeight == 0)
            {
                _targetWeights.hpWeight = 1f;
                _targetWeights.distanceWeight = 1f;
            }

            return _targetWeights;
        }
    }


    public int GetDamagePerUnitLevel(int unitLevel)
    {
        return (int)Mathf.Floor(BaseDamage * (1f + unitLevel / (float)MaxUnitLevel));
    }

    public int GetArmourPerUnitLevel(int unitLevel)
    {
        return (int)Mathf.Floor(BaseArmour * (1f + unitLevel / (float)MaxUnitLevel));
    }

    public int GetHealthPerUnitLevel(int unitLevel)
    {
        return (int)Mathf.Floor(BaseArmour * (1f + unitLevel / (float)MaxUnitLevel));
    }
}