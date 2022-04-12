
using UnityEngine;
using UnityEngine.UI;

public class Hero_click : MonoBehaviour
{
    private Hero hero;
    
    public void HeroButtonClick()
    {
        var model = hero._heroModel;
        Text health_hero = transform.Find("Health_info").GetComponent<Text>();
        Text attack_hero = transform.Find("Damage_info").GetComponent<Text>();
        Text armor_hero = transform.Find("Armor_info").GetComponent<Text>();
        Text lvl_hero = transform.Find("Lvl_info").GetComponent<Text>();
        health_hero.text = model.BaseHealth.ToString();
        attack_hero.text = model.GetUnitArmour(hero._heroLevel).ToString();
        armor_hero.text = model.GetUnitDamage(hero._heroLevel).ToString();
        lvl_hero.text = hero._heroLevel.ToString();
    }
}
