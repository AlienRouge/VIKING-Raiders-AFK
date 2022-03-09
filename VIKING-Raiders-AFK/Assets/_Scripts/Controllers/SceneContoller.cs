using System.Collections.Generic;
using UnityEngine;

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

    private SpawnPointController _spawnPointController; // Move to map controller

    private void Start()
    {
        _instance = this;

        _spawnPointController = FindObjectOfType<SpawnPointController>();

        InitializeScene(_playerHeroes, _enemyHeroes);
    }

    public void InitializeScene(List<BaseUnitModel> playerHeroes, List<BaseUnitModel> enemyHeroes)
    {
        UIController.Instance.Init(playerHeroes);
        SpawnContoller.Instance.Init(_spawnPointController);
        SpawnContoller.Instance.SpawnEnemies(_enemyHeroes);
        
    }
    
    // Start button or smth else
    public void StartBattle()
    {
        if (SpawnContoller.Instance.PlayerTeamSize>0) // Check start battle conditions
        {
            UIController.Instance.HideHeroPanel();
            BattleController.instance.StartBattle(SpawnContoller.Instance.GetSpawnedUnits());
        }
    }
}