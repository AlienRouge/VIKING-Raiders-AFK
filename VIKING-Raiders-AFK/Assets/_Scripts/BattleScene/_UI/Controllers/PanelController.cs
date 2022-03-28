using System.Collections.Generic;
using UnityEngine;

public abstract class Panel: MonoBehaviour
{
    public abstract void FillUnitPanel(List<User.Hero> heroes);
}

public class PanelController : Panel
{
    [SerializeField] private LayoutElement _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<LayoutElement> _elements;
    
    public override void FillUnitPanel(List<User.Hero> heroes)
    {
        foreach (var hero in heroes)
        {
            LayoutElement newElement = Instantiate(_layoutElementPrefab, _layoutGroup.transform, false);
            newElement.Init(hero);
            newElement.name = hero._heroModel.CharacterName;
            _elements.Add(newElement);
        }   
    }

    protected virtual LayoutElement InstantiateLayoutElement()
    {
        return Instantiate(_layoutElementPrefab, _layoutGroup.transform, false);
    }
}
