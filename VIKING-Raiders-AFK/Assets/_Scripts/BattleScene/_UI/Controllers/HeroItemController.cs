using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemController : MonoBehaviour
{
    private BaseUnitController _unit;
    
    [SerializeField] private HeroItemView _view;
    private Button _button;
    
    public void Init(BaseUnitController unit)
    {
        _unit = unit;
        
        _button = GetComponent<Button>();
        _view.SetupItemView(_unit);
        InitializeAbilityButton();
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
}
