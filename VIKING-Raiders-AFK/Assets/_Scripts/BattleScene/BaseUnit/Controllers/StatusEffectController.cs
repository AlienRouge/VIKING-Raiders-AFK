using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActualStatusEffect
{
    public BaseStatusEffect Effect;
    public float EffectDuration;
    public float NextTickTime;

    public void ResetDuration()
    {
        EffectDuration = Effect.EffectDuration;
    }

    public void ResetTickTime()
    {
        NextTickTime = Effect.TicksDeltaTime;
    }
}

public class StatusEffectController : MonoBehaviour
{
    private BaseUnitController self;
    [SerializeField] private List<ActualStatusEffect> _actualStatusEffects;
    private bool _isUnitDead;
    
    private void Awake()
    {
        self = gameObject.GetComponent<BaseUnitController>();
    }

    public void Init()
    {
        enabled = true;
    }

    public void OnUnitDead()
    {
        _isUnitDead = true;
    }

    public void AddStatusEffect(BaseStatusEffect effect)
    {
        var index = _actualStatusEffects.FindIndex(actualEffect => actualEffect.Effect == effect);

        if (index >= 0)
        {
            _actualStatusEffects[index].ResetDuration();
        }
        else
        {
            var newActualEffect = new ActualStatusEffect()
            {
                Effect = effect,
                EffectDuration = effect.EffectDuration,
                NextTickTime = 0
            };
            newActualEffect.Effect.ApplyEffect(self);
            _actualStatusEffects.Add(newActualEffect);
        }
    }

    private void RemoveActualStatusEffect(ActualStatusEffect actualStatusEffect)
    {
        actualStatusEffect.Effect.RemoveEffect(self);
        _actualStatusEffects.Remove(actualStatusEffect);
    }

    private void Update()
    {
        if (_isUnitDead) return;
       
        for (int i = _actualStatusEffects.Count - 1; i >= 0; i--)
        {
            var effect = _actualStatusEffects[i];
            effect.EffectDuration -= Time.deltaTime;
            effect.NextTickTime -= Time.deltaTime;
            
            if (effect.EffectDuration <= 0)
            {
                RemoveActualStatusEffect(effect);
            }
            
            if (effect.NextTickTime<=0)
            {
                effect.Effect.TickEffect(self);
                effect.ResetTickTime();
            }
        }
    }
}