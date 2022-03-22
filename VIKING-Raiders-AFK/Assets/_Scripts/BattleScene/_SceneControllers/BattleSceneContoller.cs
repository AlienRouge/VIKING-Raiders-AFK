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
    protected SpawnContoller _spawnController;

    private MapController _mapController;

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

    public void InitializeScene(User player, User enemy, MapController map)
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
        UIController.Instance.Show_BP(SpawnContoller.Instance.GetPlayerUnits());
        BattleController.instance.StartBattle(SpawnContoller.Instance.GetSpawnedUnits());
    }
}