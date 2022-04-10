using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_panel : MonoBehaviour
{
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private Hero_controller _layoutItemPrefab;
    private List<Hero_controller> _items;

    public void Init(List<User.Hero> userHeroes)
    {
        _items = new List<Hero_controller>();
        
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
