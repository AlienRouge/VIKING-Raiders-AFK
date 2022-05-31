using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_stats_controller : MonoBehaviour //View
{
    [SerializeField] private Text heroHealth;
    [SerializeField] private Text heroDamage;
    [SerializeField] private Text heroArmor;
    [SerializeField] private Text heroLevel;

    private void OnEnable()
    {
        Hero_controller.HeroStatsFilling += SetHeroStats;
    }

    private void OnDisable()
    {
        Hero_controller.HeroStatsFilling -= SetHeroStats;
    }

    private void SetHeroStats(Hero hero)
    {
        heroHealth.text = hero._heroModel.BaseHealth.ToString();
        heroDamage.text = hero._heroModel.BaseDamage.ToString();
        heroArmor.text = hero._heroModel.BaseArmour.ToString();
        heroLevel.text = hero._heroLevel.ToString();
        Debug.Log("Set stats");
    }
}
