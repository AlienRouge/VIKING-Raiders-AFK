using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    [SerializeField] private InventoryItemView _view;
    private User.Hero _hero;

    public void Init(User.Hero hero)
    {
        _hero = hero;

        _view.Setup(_hero);
    }
}
