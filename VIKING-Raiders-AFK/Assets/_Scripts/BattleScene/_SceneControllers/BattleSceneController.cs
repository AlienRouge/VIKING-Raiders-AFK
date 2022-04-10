using System;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    private static BattleSceneController _instance;
    
    [SerializeField] protected User _player;
    [SerializeField] protected User _enemy;

    [field: SerializeField] public BattleController BattleController { get; private set; }
    [field: SerializeField] public SpawnController SpawnController { get; private set; }
    [field: SerializeField] public UIController UIController { get; private set; }

    [SerializeField] protected MapGenerator _mapGenerator;
    protected MapController _mapController;
    
    public static BattleSceneController instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleSceneController) + "is NULL!");

            return _instance;
        }
    }


    private void Start()
    {
        _instance = this;
        
        _mapController = _mapGenerator.GenerateMap(); // TODO MIDDLEWARE
        InitializeScene(_player, _enemy, _mapController);
    }


    private void InitializeScene(User player, User enemy, MapController map)
    {
        UIController.Init(player._heroList);
        SpawnController.Init(map.spawnPointController);
        SpawnController.SpawnEnemies(enemy._heroList);
    }

    public void RestartBattle()
    {
    }

    // Start button or smth else
    public virtual void OnStartButtonHandler()
    {
        if (SpawnController.PlayerTeamSize <= 0 || SpawnController.EnemyTeamSize <= 0) return;

        EventController.BattleStarted?.Invoke();
        UIController.Show_BP(SpawnController.GetPlayerUnits());
        BattleController.StartBattle(SpawnController.GetSpawnedUnits());
    }
}