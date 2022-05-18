using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] private ShopHeroAttributeView _view;
    private BaseUnitModel _hero;
    private bool _isBought = false;
    public string Name => _hero.CharacterName;
    
    public void Init(BaseUnitModel hero)
    {
        _hero = hero;
        _view.SetupItemView(_hero);
    }

    public void SetBoughtStatus()
    {
        _isBought = true;
    }
    public void OnButtonClick()
    {
        ShopController.instance.ShowStatsPanel();
        EventController.HeroShopStatsFilling?.Invoke(_hero,_isBought);
        Debug.Log("Clicked");
    }

    public void OnCancelClick()
    {
        ShopController.instance.HideStatsPanel();
    }
}
