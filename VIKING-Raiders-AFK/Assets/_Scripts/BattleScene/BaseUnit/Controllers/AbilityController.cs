using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AbilityController : MonoBehaviour
{
    private BaseAbility _ability;
    private float _cooldownTime;
    private float _activeTime;

    enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    public void Init(BaseAbility ability)
    {
        _ability = ability;
        _state = AbilityState.Ready;
    }

    [SerializeField] private AbilityState _state;
    public UnityAction AbilityReady;
    

    public async Task ActivateAbility(BaseUnitController target)
    {
        await _ability.OnActivate(target);

        if (_ability.ActiveTime>0)
        {
            _activeTime = _ability.ActiveTime;
            _state = AbilityState.Active;
        }
        else
        {
            _cooldownTime = _ability.CooldownTime;
            _state = AbilityState.Cooldown;
        }
        
    }

    private void Update()
    {
        if (_state == AbilityState.Ready)
            return;

        switch (_state)
        {
            case AbilityState.Active:
                Debug.Log("ability active");
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    _ability.OnAbilityBeginCooldown();
                    _state = AbilityState.Cooldown;
                    _cooldownTime = _ability.CooldownTime;
                }

                break;

            case AbilityState.Cooldown:
                Debug.Log("ability cooldown");
                if (_cooldownTime > 0)
                {
                    _cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.Ready;
                    AbilityReady?.Invoke();
                }

                break;

            case AbilityState.Ready:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}