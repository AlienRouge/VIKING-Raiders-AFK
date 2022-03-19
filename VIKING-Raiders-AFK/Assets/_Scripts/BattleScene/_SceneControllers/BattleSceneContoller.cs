using System.Collections.Generic;
using UnityEngine;

public class BattleSceneContoller : MonoBehaviour
{
    protected static BattleSceneContoller _instance;

    public static BattleSceneContoller instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleSceneContoller) + "is NULL!");

            return _instance;
        }
    }

    [SerializeField] protected User _player;
    [SerializeField] protected User _enemy;
    protected SpawnContoller _spawnController;

    protected MapController _mapController;

    private void Start()
    {
        _instance = this;

        _mapController = MapGenerator.instance.GenerateMap(); // To upper controller
        SetSpawnController();
        InitializeScene(_player, _enemy, _mapController);
    }

    protected virtual void SetSpawnController()
    {
        _spawnController = SpawnContoller.Instance;
    }

    protected void InitializeScene(User player, User enemy, MapController map)
    {
        UIController.Instance.Init(player._heroList);
        _spawnController.Init(map.spawnPointController);
        _spawnController.SpawnEnemies(enemy._heroList);
        
    }
    
    // Start button or smth else
    public void OnStartButtonHandler()
    {
        Debug.Log(_spawnController);
        if (_spawnController.PlayerTeamSize <= 0) return;
        
        EventController.BattleStarted?.Invoke();
        BattleController.instance.StartBattle(SpawnContoller.Instance.GetSpawnedUnits());
    }
}