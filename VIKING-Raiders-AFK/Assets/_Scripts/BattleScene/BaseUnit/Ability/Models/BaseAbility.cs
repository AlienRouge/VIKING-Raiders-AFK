using UnityEngine;

public class BaseAbility : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float ActiveTime { get; private set; }
    [field: SerializeField] public float CastRange { get; private set; }

    public virtual void OnActivate(BaseUnitController target)
    {
    }
    
}