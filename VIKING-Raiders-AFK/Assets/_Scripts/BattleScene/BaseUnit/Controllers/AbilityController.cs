using UnityEngine;

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

    private AbilityState _state;


    private void ActivateAbility(BaseUnitController target)
    {
        switch (_state)
        {
            case AbilityState.Ready:
                _ability.OnActivate(target);
                _state = AbilityState.Active;
                _activeTime = _ability.ActiveTime;
                break;
            case AbilityState.Active:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.Cooldown;
                    _cooldownTime = _ability.CooldownTime;
                }
                break;
            case AbilityState.Cooldown:
                if (_cooldownTime > 0)
                {
                    _cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.Ready;
                }
                break;
        }
    }
}
