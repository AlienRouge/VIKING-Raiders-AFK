using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class InventoryItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _attack;
    [SerializeField] private TextMeshProUGUI _armour;
    [SerializeField] private TextMeshProUGUI _Level;
    [SerializeField] private Image _image;

    public void Setup(Hero hero)
    {
        var model = hero._heroModel;
        _name.text = model.CharacterName;
        _image.sprite = model.ViewSprite;
        _health.text = model.BaseHealth.ToString();
        _armour.text = model.GetUnitArmour(hero._heroLevel).ToString();
        _attack.text = model.GetUnitDamage(hero._heroLevel).ToString();
        _Level.text = hero._heroLevel.ToString();
    }
}