using System;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    public static UIController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(UIController) + "is NULL!");

            return _instance;
        }
    }
    
    [SerializeField] private PanelController _panelController;
    
    private bool _isVisible;

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        EventController.BattleStarted += SwitchSpawnUIVisible;
        EventController.UnitDragged += (arg0 => SwitchSpawnUIVisible());
    }

    private void OnDisable()
    {
        EventController.BattleStarted -= SwitchSpawnUIVisible;
        EventController.UnitDragged -= (arg0 => SwitchSpawnUIVisible());
    }

    public void Init(List<User.Hero> playerUnitModels)
    {
        _isVisible = true;
        _panelController.FillUnitPanel(playerUnitModels);
    }
    
    public void SwitchSpawnUIVisible()
    {
        _isVisible = !_isVisible;
        _panelController.gameObject.SetActive(_isVisible);
    }
}
