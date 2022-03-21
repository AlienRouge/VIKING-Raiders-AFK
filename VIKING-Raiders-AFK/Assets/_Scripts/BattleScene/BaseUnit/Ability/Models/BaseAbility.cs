using System.Threading.Tasks;
using UnityEngine;

public class BaseAbility : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    [field: SerializeField] public float CooldownTime { get; private set; }
    [field: SerializeField] public float ActiveTime { get; private set; }
    [field: SerializeField] public float CastRange { get; private set; }
    [field: SerializeField] public float CastTime { get; private set; }

    public virtual async Task OnActivate(BaseUnitController target)
    {
        await Task.Delay(Mathf.RoundToInt( CastTime * 1000));
    }
    
    public virtual void OnAbilityBeginCooldown()
    {
    }
}