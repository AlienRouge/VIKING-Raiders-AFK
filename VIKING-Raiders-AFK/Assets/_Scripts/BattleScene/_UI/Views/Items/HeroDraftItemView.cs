using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDraftItemView : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _attack;
    [SerializeField] private TextMeshProUGUI _armour;
    [SerializeField] private TextMeshProUGUI _Level;
    [SerializeField] private Image _image;

    [SerializeField] private Color _selectedColor;
    private Color _commonColor;
    
    public void Setup(Hero hero)
    {
        var model = hero._heroModel;
        _name.text = model.CharacterName;
        _image.sprite = model.ViewSprite;
        _armour.text = model.GetUnitArmour(hero._heroLevel).ToString();
        _attack.text = model.GetUnitDamage(hero._heroLevel).ToString();
        _Level.text = hero._heroLevel.ToString();

        _commonColor = _background.color;
    }

    public void HighlightItem(bool isSelected)
    {
        _background.color = isSelected ? _selectedColor : _commonColor;
    }
}
