using System;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private User _userData;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _userlvl;
    [SerializeField] private SceneLoader _sceneLoader;
    
    private void Start()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        _userName.text = _userData._userName;
        _userlvl.text = $"{_userData._accountLevel} lvl";
    }

    public void PlayGame()
    {
        MapGenerator.Seed = (int)DateTime.Now.Ticks;
        _sceneLoader.Load("BattleScene");
    }
    public void GoInventory()
    {
        _sceneLoader.Load("InventoryScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
