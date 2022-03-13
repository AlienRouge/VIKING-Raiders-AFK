using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class PanelController : MonoBehaviourPunCallbacks
{
    [SerializeField] protected LayoutElement _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<LayoutElement> _elements;
    
    protected delegate void SpawnAction();

    protected SpawnAction SpawnUnit;
    public void FillUnitPanel(List<BaseUnitModel> unitModels)
    {
        foreach (var model in unitModels)
        {
            LayoutElement newElement = InstatiateLayoutElement();
            newElement.Init(model);
            newElement.name = model.characterName;
            newElement.transform.parent = _layoutGroup.transform;
            _elements.Add(newElement);
        }   
    }

    protected virtual LayoutElement InstatiateLayoutElement()
    {
        return Instantiate(_layoutElementPrefab);
    }
}
