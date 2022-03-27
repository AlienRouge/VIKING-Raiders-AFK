using System.Collections.Generic;
using _Scripts.Network.UI;
using Photon.Pun;
using UnityEngine;

public class PanelControllerNet : Panel
{
    [SerializeField] private LayoutElementNet _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<LayoutElementNet> _elements;
    
    public override void FillUnitPanel(List<User.Hero> heroes)
    {
        foreach (var hero in heroes)
        {
            LayoutElementNet newElement = Instantiate(_layoutElementPrefab, _layoutGroup.transform, false);
            Debug.Log(newElement.GetType());
            newElement.Init(hero);
            newElement.name = hero._heroModel.CharacterName;
            _elements.Add(newElement);
        }   
    }
}