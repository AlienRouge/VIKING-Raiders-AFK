using UI.Scripts;
using UnityEngine;


public class Hero_controller : MonoBehaviour
{
    [SerializeField] private Heroes_attributes _view;
    private User.Hero _hero;

    public void Init(User.Hero hero)
    {
        _hero = hero;
        _view.SetupItemView(_hero);
    }
    
    public void OnButtonClick()
    {
        hero_scene.instance.ShowStatsPanel();
        EventController.HeroStatsFilling?.Invoke(_hero);
        Debug.Log("Clicked");
    }

    public void OnCancelClick()
    {
        hero_scene.instance.HideStatsPanel();
    }
}
