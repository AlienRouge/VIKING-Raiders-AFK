using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private LayoutElement _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private List<LayoutElement> _elements;
    
    public void FillUnitPanel(List<BaseUnitModel> unitModels)
    {
        foreach (var model in unitModels)
        {
            LayoutElement newElement = Instantiate(_layoutElementPrefab);
            newElement.Init(model);
            newElement.name = model.characterName;
            newElement.transform.parent = _layoutGroup.transform;
            _elements.Add(newElement);
        }   
    }
}
