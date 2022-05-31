using UI.Scripts;
using UnityEngine;
using UnityEngine.Events;


public class Hero_controller : MonoBehaviour //StatsPanelCotroler
{
    [SerializeField] private Heroes_attributes _view;
    public static UnityAction<Hero> HeroStatsFilling;
    private Hero _hero;

    public void Init(Hero hero)
    {
        _hero = hero;
        _view.SetupItemView(_hero);
    }
    
    public void OnButtonClick()
    {
        hero_scene.instance.ShowStatsPanel();
        HeroStatsFilling?.Invoke(_hero);
        Debug.Log("Clicked");
    }

    public void OnCancelClick()
    {
        hero_scene.instance.HideStatsPanel();
    }
}
