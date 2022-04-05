using System.Collections.Generic;
using UnityEngine;

public class HeroDraftController : MonoBehaviour
{
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private HeroDraftItemController _layoutItemPrefab;
    private List<HeroDraftItemController> _items;
    
    private bool _isVisible;
    private bool _isHidden;
    
    public void Init(List<User.Hero> playerHeroes)
    {
        _items = new List<HeroDraftItemController>();
        _isVisible = true;
        _isHidden = false;

        foreach (var hero in playerHeroes)
        {
            AddItem(hero);
        }
    }
    
    private void AddItem(User.Hero hero)
    {
        var newItem = Instantiate(_layoutItemPrefab, _layoutGroup.transform, false);
        newItem.Init(hero);
        
        _items.Add(newItem);
    }

    public void HidePanel()
    {
        _isHidden = true;
        _isVisible = false;
        gameObject.SetActive(_isVisible);
    }
    
    public void SwitchPanelHideStatus()
    {
        _isHidden = !_isHidden;
        gameObject.SetActive(!_isHidden);
    }
    public void SwitchPanelVisibility()
    {
        if (_isHidden) return;
        
        _isVisible = !_isVisible;
        gameObject.SetActive(_isVisible);
    }
}
