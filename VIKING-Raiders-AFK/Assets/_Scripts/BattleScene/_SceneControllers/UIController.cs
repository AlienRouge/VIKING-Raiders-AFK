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
    
    [SerializeField] private Panel _panelController;
    [SerializeField] private AbilitiesPanelController abilitiesPanelController;
    
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

    public void Init(List<User.Hero> playerHeroes)
    {
        _isVisible = true;
        _panelController.FillUnitPanel(playerHeroes);
        
    }

    public void SwitchSpawnUIVisible()
    {
        _isVisible = !_isVisible;
        _panelController.gameObject.SetActive(_isVisible);
    }

    public void Show_BP(List<BaseUnitController> list)
    {
        Debug.Log(abilitiesPanelController);
        abilitiesPanelController.Init(list);
    }
}
