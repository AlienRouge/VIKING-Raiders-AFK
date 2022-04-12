using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemController : MonoBehaviour
{
    private BaseUnitController _unit;
    private BaseUnitModel _unitModel;
    
    [SerializeField] private HeroItemView _view;
    private Button _button;
    
    public void Init(BaseUnitController unit)
    {
        _button = GetComponent<Button>();
        
        _unit = unit;
        _unitModel = _unit.ActualStats.Model;
        
        _view.SetupItemView(_unitModel);
        
        if (_unitModel.ActiveAbility)
        {
            InitializeAbilityButton();
        }
        
        gameObject.SetActive(true);
    }

    private void InitializeAbilityButton()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        EventController.UseUnitActiveAbility?.Invoke(_unit);
    }

    private void OnAbilityStateChange(BaseUnitController unit, AbilityController.AbilityState state)
    {
        if (!ReferenceEquals(_unit, unit)) return;

        switch (state)
        {
            case AbilityController.AbilityState.Ready:
                _view.OnAbilityReady();
                break;
            case AbilityController.AbilityState.Active:
                _view.OnAbilityActive();
                break;
            case AbilityController.AbilityState.Cooldown:
                _view.OnAbilityCooldown();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnBattleEnded()
    {
        _button.enabled = false;
    }

    private void OnUnitDied(BaseUnitController unit)
    {
        if (!ReferenceEquals(_unit, unit)) return;

        _button.enabled = false;
        _view.UnitDied();
    }
    
    private void OnEnable()
    {
        EventController.ActiveAbilityStateChanged += OnAbilityStateChange;
        EventController.UnitDied += OnUnitDied;
        EventController.BattleEnded += OnBattleEnded;
    }

    private void OnDisable()
    {
        EventController.ActiveAbilityStateChanged -= OnAbilityStateChange;
        EventController.UnitDied -= OnUnitDied;
        EventController.BattleEnded -= OnBattleEnded;
    }
}
