using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.BattleScene._SceneControllers;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopController : MonoBehaviour
{
    private ModelsContainer _containerData;
    
    [FormerlySerializedAs("_panelController")] [SerializeField] private ShopPanelView panelViewController;
    [SerializeField] private Hero_stats_panel _heroStats;
    private SceneLoader _sceneLoader;
    private static ShopController _instance;
    [SerializeField] private User _user;
    private TradeController _tradeController;
    private List<string> _listOfBoughtItems ;

    public static ShopController instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(ShopController) + "is NULL!");

            return _instance;
        }
    }
    private void Start()
    {
        _tradeController = new TradeController(_user);
        _instance = this;
        _sceneLoader = GetComponent<SceneLoader>();
        _containerData = Resources.Load<ModelsContainer>("ShopModels");
        var shopModels = _containerData.GetIntersectListModel(_user.heroList);
        _listOfBoughtItems = new List<string>(shopModels.Count);
        FillInventoryPanel(shopModels);
    }

    public void BackToMainMenuScene()
    {
        _sceneLoader.Load("MainMenuScene");
    }

    private void FillInventoryPanel(List<BaseUnitModel> models)
    {
        panelViewController.Init(models);
    }
    
    public void ShowStatsPanel()
    {
        _heroStats.gameObject.SetActive(true);
    }

    public void HideStatsPanel()
    {
        _heroStats.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        ShopEventController.ShopBuyAction += OnShopBuyAction;
    }

    public void OnDisable()
    {
        ShopEventController.ShopBuyAction -= OnShopBuyAction;
    }
    
    private void OnShopBuyAction(BaseUnitModel target)
    {
        if (_tradeController.CanBuy(target))
        {
            _tradeController.Buy(target);
            _listOfBoughtItems.Add(target.CharacterName);
            panelViewController.SetItemBoughtStatus(target.CharacterName);
        }
        else
        {
            Debug.Log("Net deneg!!!");
        }
    }
}
