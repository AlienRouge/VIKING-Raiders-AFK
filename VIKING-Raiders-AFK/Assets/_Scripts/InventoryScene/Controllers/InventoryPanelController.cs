using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private InventoryItemController _layoutItemPrefab;
    private List<InventoryItemController> _items;

    public void Init(List<User.Hero> userHeroes)
    {
        _items = new List<InventoryItemController>();
        
        foreach (var hero in userHeroes)
        {
            FillPanel(hero);
        }
    }

    private void FillPanel(User.Hero hero)
    {
        var newItem = Instantiate(_layoutItemPrefab, _layoutGroup.transform, false);
        newItem.Init(hero);
        
        _items.Add(newItem);
    }
}
