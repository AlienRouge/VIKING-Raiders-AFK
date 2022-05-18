using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopStatsPanel : MonoBehaviour
{
    [SerializeField] private Text heroHealth;
    [SerializeField] private Text heroDamage;
    [SerializeField] private Text heroArmor;
    [SerializeField] private Text heroLevel;
    [SerializeField] private Text heroPrice;

    private void OnEnable()
    {
        EventController.HeroShopStatsFilling += SetHeroStats;
    }

    private void OnDisable()
    {
        EventController.HeroShopStatsFilling -= SetHeroStats;
    }

    private void SetHeroStats(BaseUnitModel hero, bool isBought)
    {
        heroHealth.text = hero.BaseHealth.ToString();
        heroDamage.text = hero.BaseDamage.ToString();
        heroArmor.text = hero.BaseArmour.ToString();
        heroLevel.text = hero.CharacterName;
       // heroPrice.text = hero.CharacterPrice.ToString();
        Debug.Log("Set stats");
    }
}
