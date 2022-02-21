using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SceneContoller : MonoBehaviour
{
    private static SceneContoller _instance;
    public static SceneContoller instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SceneContoller) + "is NULL!");

            return _instance;
        }
    }
    
    [SerializeField] private List<BaseUnitModel> _playerHeroes;
    [SerializeField] private List<BaseUnitModel> _enemyHeroes;

    private PanelController _panelController; // To UI controller
    private void Start()
    {
        _instance = this; 
        _panelController = FindObjectOfType<PanelController>();
        
        InitializeScene(_playerHeroes, _enemyHeroes);
    }

    public void InitializeScene(List<BaseUnitModel> playerHeroes, List<BaseUnitModel> enemyHeroes)
    {
        _panelController.Init(playerHeroes);
        
    }

    // Start button or smth else
    public void StartBattle()
    {
        if (true) // Check start battle conditions
        {
            BattleController.instance.StartBattle(_playerHeroes, _enemyHeroes);
        }
    }
}
