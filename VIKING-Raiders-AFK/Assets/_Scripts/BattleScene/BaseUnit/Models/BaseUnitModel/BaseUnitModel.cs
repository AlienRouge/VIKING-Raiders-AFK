using System;
using System.ComponentModel;
using _Scripts.Enums;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public struct TargetWeights
{
    public float HpWeight;
    public float DistanceWeight;
}

public abstract class BaseUnitModel : ScriptableObject
{
    [field: Header("UnitDescription")]
    [field: SerializeField]
    public string CharacterName { get; private set; }
    [field: SerializeField]
    public int CharacterPrice { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField, Header("View Sprite")]
    public Sprite ViewSprite { get; private set; }
    
    [field: SerializeField, Header("Avatar Sprite")]
    public Sprite AvatarSprite { get; private set; }
    
    [field: SerializeField, Header("Animation Controller")]
    public AnimatorController AnimatorController { get; private set; }
    
    [field: SerializeField] public Vector3 ViewSpriteScale { get; private set; }

    
    [field: Header("Base attributes")]
    [field: SerializeField]
    public int MaxUnitLevel { get; private set; }

    [field: SerializeField] public int BaseHealth { get; private set; }
    [field: SerializeField] public int BaseDamage { get; private set; }
    [field: SerializeField] public int BaseArmour { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }

    
    [field: Header("Attack attributes")]
    [field: SerializeField, DefaultValue(1)]
    public float AttackDeltaTime { get; private set; }

    [field: SerializeField, Range(1, 20)] public float AttackRange { get; private set; }
    [field: SerializeField, Range(0, 1)] public float CritChance { get; private set; }

    [field: SerializeField] public float MinCritrate { get; private set; }

    [field: SerializeField] public float MaxCritrate { get; private set; }

    
    [field: Header("Abilities")]
    [field: SerializeField]
    public BaseAbility PassiveAbility { get; private set; }

    [field: SerializeField] public BaseAbility ActiveAbility { get; private set; }

    [field: Header("Target weights")] [SerializeField]
    private TargetWeights _targetWeights;


    public TargetWeights TargetWeights
    {
        get
        {
            if (_targetWeights.HpWeight + _targetWeights.DistanceWeight == 0)
            {
                _targetWeights.HpWeight = 1f;
                _targetWeights.DistanceWeight = 1f;
            }

            return _targetWeights;
        }
    }

    public abstract ProjectileModel GetProjModel();

    public int GetUnitArmour(int unitLevel)
    {
        return (int)Mathf.Floor(BaseArmour * (1f + unitLevel / (float)MaxUnitLevel));
    }

    public int GetUnitDamage(int unitLevel)
    {
        return (int)Mathf.Floor(BaseDamage * (1f + unitLevel / (float)MaxUnitLevel));
    }

    public int GetHealthPerUnitLevel(int unitLevel)
    {
        return (int)Mathf.Floor(BaseArmour * (1f + unitLevel / (float)MaxUnitLevel));
    }
}