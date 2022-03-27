using UnityEngine;
using _Scripts.Network.Map;
using Photon.Pun;

public class BattleSceneControllerNet : BattleSceneController
{
    [SerializeField] private MapGeneratorNet mapGenerator;

    private static BattleSceneControllerNet _instance;

    public static BattleSceneControllerNet instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(BattleSceneControllerNet) + "is NULL!");

            return _instance;
        }
    }
    
    private void Start()
    {
        _instance = this;
        SetSpawnController();
    }
    protected override void SetSpawnController()
    {
        _spawnController = SpawnControllerNet.Instance;
    }

    public void SetMapController(MapController mapController)
    {
        _mapController = mapController;
    }

    public void ShowUi()
    {
        UIController.Instance.Init(_player._heroList);
        _spawnController.Init(_mapController.spawnPointController);
    }

    public void InitScene()
    {
        _mapController = mapGenerator.GenerateMap();
        ShowUi();
        /*InitializeScene(_player, _enemy, _mapController);*/
    }
    
    public override void OnStartButtonHandler()
    {
        if (_spawnController.PlayerTeamSize <= 0 || _spawnController.EnemyTeamSize <= 0) return;
        
        EventController.BattleStarted?.Invoke();
        UIController.Instance.Show_BP(SpawnControllerNet.Instance.GetPlayerUnits());
        BattleController.instance.StartBattle(SpawnControllerNet.Instance.GetSpawnedUnits());
    }
}