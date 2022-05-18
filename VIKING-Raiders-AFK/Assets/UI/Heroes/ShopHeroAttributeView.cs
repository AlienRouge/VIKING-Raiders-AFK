using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHeroAttributeView : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _image;

    public void SetupItemView(BaseUnitModel hero)
    {
        //var model = hero._heroModel;
        _name.text = hero.CharacterName;
        _image.sprite = hero.ViewSprite;
    }
}
