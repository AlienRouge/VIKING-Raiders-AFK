using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private List<LayoutElement> _elements;
    [SerializeField] private LayoutElement _layoutElementPrefab;
    [SerializeField] private GameObject _layoutGroup;
    
    public void Init(List<BaseUnitModel> unitModels)
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
