using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _instance = this;
    }
    
    public void Init(List<BaseUnitModel> playerUnitModels)
    {
        _panelController.FillUnitPanel(playerUnitModels);
    }

    public void HideHeroPanel()
    {
        _panelController.gameObject.SetActive(false);
    }
}
