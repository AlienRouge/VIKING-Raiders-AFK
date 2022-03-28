using System.Collections.Generic;
using UnityEngine;

public class AbilitiesPanelController : MonoBehaviour
{
    [SerializeField] private List<HeroItemController> _heroItems;

    public void Init(List<BaseUnitController> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            _heroItems[i].Init(units[i]);
        }
    }
}