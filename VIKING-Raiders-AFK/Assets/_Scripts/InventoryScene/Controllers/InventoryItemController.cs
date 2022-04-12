using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    [SerializeField] private InventoryItemView _view;
    private Hero _hero;

    public void Init(Hero hero)
    {
        _hero = hero;

        _view.Setup(_hero);
    }
}
