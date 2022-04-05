using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private HeroDraftController _heroDraftController;
    [SerializeField] private AbilitiesPanelController abilitiesPanelController;
    [SerializeField] private Button _hideDraftPanelBtn;
    
    public void Init(List<User.Hero> playerHeroes)
    {
        _heroDraftController.Init(playerHeroes);
    }
    
    public void Show_BP(List<BaseUnitController> list)
    {
        abilitiesPanelController.Init(list);
    }

    public void OnPanelHideButtonHandler()
    {
        _heroDraftController.SwitchPanelHideStatus();
    }
    
    private void OnUnitDraggedHandler(Team team)
    {
        _heroDraftController.SwitchPanelVisibility();
    }

    private void OnStartBattleHandler()
    {
        _heroDraftController.HidePanel();
        _hideDraftPanelBtn.gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        EventController.BattleStarted += OnStartBattleHandler;
        EventController.UnitDragged += OnUnitDraggedHandler;
    }

    private void OnDisable()
    {
        EventController.BattleStarted -= OnStartBattleHandler;
        EventController.UnitDragged -= OnUnitDraggedHandler;
    }
}
