using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero_scene : MonoBehaviour
{
    private User _userData;
    
    [SerializeField] private Hero_panel _panelController;

    [SerializeField] private Hero_stats_panel _heroStats;
    private SceneLoader _sceneLoader;
    private static hero_scene _instance;
    
    public static hero_scene instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(hero_scene) + "is NULL!");

            return _instance;
        }
    }
    private void Start()
    {
        _instance = this;
        _sceneLoader = GetComponent<SceneLoader>();
        _userData = Resources.Load<User>("Player");
        FillInventoryPanel();
    }

    public void BackToMainMenuScene()
    {
        _sceneLoader.Load("MainMenuScene");
    }

    private void FillInventoryPanel()
    {
        _panelController.Init(_userData._heroList);
    }
    
    public void ShowStatsPanel()
    {
        _heroStats.gameObject.SetActive(true);
    }

    public void HideStatsPanel()
    {
        _heroStats.gameObject.SetActive(false);
    }
}
