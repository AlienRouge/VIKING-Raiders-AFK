using System.Collections.Generic;
using UnityEngine;

public class HeroDraftController : MonoBehaviour
{
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private HeroDraftItemController _layoutItemPrefab;
    private List<HeroDraftItemController> _items;
    
    private bool _isVisible;
    public bool IsHidden;
    
    public void Init(List<Hero> playerHeroes)
    {
        _items = new List<HeroDraftItemController>();
        _isVisible = true;
        IsHidden = false;

        foreach (var hero in playerHeroes)
        {
            AddItem(hero);
        }
    }
    
    private void AddItem(Hero hero)
    {
        var newItem = Instantiate(_layoutItemPrefab, _layoutGroup.transform, false);
        newItem.Init(hero);
        
        _items.Add(newItem);
    }

    public void HidePanel()
    {
        IsHidden = true;
        _isVisible = false;
        gameObject.SetActive(_isVisible);
    }
    
    public void SwitchPanelHideStatus()
    {
        IsHidden = !IsHidden;
        gameObject.SetActive(!IsHidden);
    }
    public void SwitchPanelVisibility()
    {
        if (IsHidden) return;
        
        _isVisible = !_isVisible;
        gameObject.SetActive(_isVisible);
    }
}
