using UnityEngine;

public abstract class BaseStatusEffect : ScriptableObject
{
    [field: SerializeField] public string StatusName { get; private set; }
    [field: SerializeField] public string DiscriptionName { get; private set; }
    [field: SerializeField] public float EffectDuration { get; private set; }
    [field: SerializeField] public float TicksDeltaTime { get; private set; }

    public void ApplyEffect(BaseUnitController target)
    {
        DoOnApplyEffect(target);
    }
    
    public void TickEffect(BaseUnitController target)
    {
        DoOnTickEffect(target);
    }
    
    public void RemoveEffect(BaseUnitController target)
    {
        DoOnRemoveEffect(target);
    }

    protected abstract void DoOnApplyEffect(BaseUnitController target);
    protected abstract void DoOnTickEffect(BaseUnitController target);
    protected abstract void DoOnRemoveEffect(BaseUnitController target);
}
