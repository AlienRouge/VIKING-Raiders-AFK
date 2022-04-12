using UnityEngine;
using UnityEngine.UI;

public class Heroes_attributes : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _image;

    public void SetupItemView(Hero hero)
    {
        var model = hero._heroModel;
        _name.text = model.CharacterName;
        _image.sprite = model.ViewSprite;
    }
}
