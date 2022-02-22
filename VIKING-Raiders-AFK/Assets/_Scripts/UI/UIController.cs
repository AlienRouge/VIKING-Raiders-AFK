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
                Debug.LogError(nameof(SceneContoller) + "is NULL!");

            return _instance;
        }
    }
    
    [SerializeField] private PanelController _panelController;

    public void Init(List<BaseUnitModel> playerUnitModels)
    {
        _panelController.FillUnitPanel(playerUnitModels);
    }
    private void Start()
    {
        _instance = this;
    }
}
