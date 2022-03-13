using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    public static SceneController instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SceneController) + "is NULL!");

            return _instance;
        }
    }

    [SerializeField] private List<BaseUnitModel> _playerHeroes;
    [SerializeField] private List<BaseUnitModel> _enemyHeroes;

    private MapController _mapController;

    private SpawnPointController _spawnPointController; // Move to map controller
    protected SpawnController _spawnController;

    private void Start()
    {
        _instance = this;

        _mapController = MapGenerator.instance.GenerateMap();
        _spawnPointController = _mapController.spawnPointController;
        SetSpawnController();

        InitializeScene(_playerHeroes, _enemyHeroes);
    }

    protected virtual void SetSpawnController()
    {
        _spawnController = SpawnController.Instance;
    }

    public void InitializeScene(List<BaseUnitModel> playerHeroes, List<BaseUnitModel> enemyHeroes)
    {
        UIController.Instance.Init(playerHeroes);
        _spawnController.Init(_spawnPointController);
        _spawnController.SpawnEnemies(_enemyHeroes);
        
    }
    
    // Start button or smth else
    public void StartBattle()
    {
        if (_spawnController.PlayerTeamSize>0) // Check start battle conditions
        {
            UIController.Instance.HideHeroPanel();
            BattleController.instance.StartBattle(_spawnController.GetSpawnedUnits());
        }
    }
}