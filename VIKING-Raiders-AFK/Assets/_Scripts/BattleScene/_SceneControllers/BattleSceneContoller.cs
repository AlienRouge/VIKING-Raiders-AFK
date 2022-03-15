using System.Collections.Generic;
using UnityEngine;

public class BattleSceneContoller : MonoBehaviour
{
    private static BattleSceneContoller _instance;

    public static BattleSceneContoller instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleSceneContoller) + "is NULL!");

            return _instance;
        }
    }

    [SerializeField] private User _player;
    [SerializeField] private User _enemy;

    private MapController _mapController;

    private void Start()
    {
        _instance = this;

        _mapController = MapGenerator.instance.GenerateMap(); // To upper controller
        InitializeScene(_player, _enemy, _mapController);
    }

    public void InitializeScene(User player, User enemy, MapController map)
    {
        UIController.Instance.Init(player._heroList);
        SpawnContoller.Instance.Init(map.spawnPointController);
        SpawnContoller.Instance.SpawnEnemies(enemy._heroList);
        
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