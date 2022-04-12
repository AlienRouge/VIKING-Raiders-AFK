using UnityEngine;
using _Scripts.Network.Map;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class BattleSceneControllerNet : BattleSceneController, IOnEventCallback
{
    private static BattleSceneControllerNet _instance;

    public static BattleSceneControllerNet Instance
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
        // SetSpawnController();
    }
    // protected override void SetSpawnController()
    // {
    //     _spawnController = SpawnControllerNet.Instance;
    // }

    public void SetMapController(MapController mapController)
    {
        _mapController = mapController;
    }

    public void ShowUi()
    {
        UIController.Init(_playerData.heroList);
        SpawnController.Init(_mapController.SpawnPointsController);
        // _spawnController.Init(_mapController.spawnPointController);
    }

    public void InitScene()
    {
        _mapController = _mapGenerator.GenerateMap();
        ShowUi();
    }
    
    public override void OnStartButtonHandler()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // if (_spawnController.PlayerTeamSize <= 0 || _spawnController.EnemyTeamSize <= 0) return;
        if (SpawnController.PlayerTeamSize <= 0 || SpawnController.EnemyTeamSize <= 0) return;
        
        EventController.BattleStarted?.Invoke();
        UIController.Show_BP(SpawnControllerNet.Instance.GetPlayerUnits());
        BattleController.StartBattle(SpawnControllerNet.Instance.GetSpawnedUnits());
        SendStartBattle();
    }

    private void SendStartBattle()
    {
        var riseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.Others
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent((byte) NetEvents.BeginFight, null, riseEventOptions, sendOptions);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte) NetEvents.BeginFight:
            {
                EventController.BattleStarted?.Invoke();
                UIController.Show_BP(SpawnControllerNet.Instance.GetPlayerUnits());
                BattleController.StartBattle(SpawnControllerNet.Instance.GetSpawnedUnits());
                //InstantiateReceivedModal((SyncData) photonEvent.CustomData);
                break;
            }
            default:
                return;
        }
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}