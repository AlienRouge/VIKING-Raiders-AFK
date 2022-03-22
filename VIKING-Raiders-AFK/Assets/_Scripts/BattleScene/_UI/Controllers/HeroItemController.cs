using TMPro;
using UnityEngine;

public class HeroItemController : MonoBehaviour
{
    [SerializeField] private HeroItemView _view;

    public void Init(BaseUnitController unit)
    {
        _view.SetupItemView(unit);
    }
}
