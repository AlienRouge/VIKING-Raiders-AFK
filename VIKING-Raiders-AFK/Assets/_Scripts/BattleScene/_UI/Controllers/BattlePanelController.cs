using System;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanelController : MonoBehaviour
{

    [SerializeField] private HeroItemController _heroItemPrefab;

    public void Init(List<BaseUnitController> units)
    {
        foreach (var unit in units)
        {
           FillPanel(unit);
        }
    }

    private void FillPanel(BaseUnitController unit)
    {
        var newItem =  Instantiate(_heroItemPrefab, transform, false);
        newItem.Init(unit);
    }
}
