using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private LayoutElement _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<LayoutElement> _elements;
    
    public void FillUnitPanel(List<User.Hero> heroes)
    {
        foreach (var hero in heroes)
        {
            LayoutElement newElement = Instantiate(_layoutElementPrefab, _layoutGroup.transform, false);
            newElement.Init(hero);
            newElement.name = hero._heroModel.CharacterName;
            _elements.Add(newElement);
        }   
    }
}