using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelView : MonoBehaviour
{
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private ShopItemView _layoutItemPrefab; 
    private List<ShopItemView> _items;

    public void Init(List<BaseUnitModel> shopHeroes)
    {
        _items = new List<ShopItemView>();
        
        
        foreach (var hero in shopHeroes)
        {
            FillPanel(hero);
        }
    }

    private void FillPanel(BaseUnitModel hero)
    {
        var newItem = Instantiate(_layoutItemPrefab, _layoutGroup.transform, false);
        newItem.Init(hero);
        _items.Add(newItem);
    }

    public void SetItemBoughtStatus(string name)
    {
        foreach (var item in _items)
        {
            if (item.Name == name)
            {
                item.SetBoughtStatus();
                break;
            }
        }
    }
}
