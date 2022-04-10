using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Heroes_attributes : MonoBehaviour
{
    [SerializeField] private Text _name;
    // [SerializeField] private Text _health;
    // [SerializeField] private Text _attack;
    // [SerializeField] private Text _armour;
    // [SerializeField] private Text _Level;
    [SerializeField] private Image _image;
   // [SerializeField] public Button clickButton;

    public void SetupItemView(User.Hero hero)
    {
        var model = hero._heroModel;
        _name.text = model.CharacterName;
        _image.sprite = model.ViewSprite;
        // _health.text = model.BaseHealth.ToString();
        // _armour.text = model.GetArmourPerUnitLevel(hero._heroLevel).ToString();
        // _attack.text = model.GetDamagePerUnitLevel(hero._heroLevel).ToString();
        // _Level.text = hero._heroLevel.ToString();
    }
    
    
}
