using System.Threading.Tasks;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] protected float _cooldown;
    public float cooldown => _cooldown;

    [SerializeField] protected float _powerModifier;
    public float powerModifier => _powerModifier;
    
    [SerializeField] protected float _baseDamage;
    public float baseDamage => _baseDamage;

    [SerializeField] protected float _castTime;
    public float castTime => _castTime;
    
    public float currentDamage => baseDamage * powerModifier;
    
    public abstract Task Use(BaseUnitController target);
    public abstract bool CheckPossibilityToUseAbility();
    public abstract void Init();
}
