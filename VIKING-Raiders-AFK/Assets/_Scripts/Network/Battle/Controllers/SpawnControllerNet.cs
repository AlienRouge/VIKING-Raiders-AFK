using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Network.SyncData;
using _Scripts.Network.UnitController;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnControllerNet : SpawnController, IOnEventCallback
{
    [SerializeField] private ModelsContainer _models;
    [SerializeField] private BaseUnitController _netModel;
    private static SpawnControllerNet _instance;

    public static SpawnControllerNet Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SpawnControllerNet) + " is NULL!");

            return _instance;
        }
    }

    private Team EnemyTeam => CurrentTeam == Team.Team1 ? Team.Team2 : Team.Team1;

    private void Awake()
    {
        _instance = this;
        _playerTeam = new List<SpawnedUnit>();
    }

    private void Start()
    {
        _models.Init();
        SpawnUnit = InstantiateUnitNet;
        if (PhotonNetwork.IsMasterClient)
        {
            CurrentTeam = Team.Team1;
        }
        else
        {
            CurrentTeam = Team.Team2;
        }
    }


    //TODO Добавить синхронизацию начального перемещения юнита у клиента
    private void InstantiateUnitNet(Team team, User.Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        SyncData syncData = new SyncData()
        {
            heroLevel = hero._heroLevel,
            modalName = hero._heroModel.name,
            spawnPos = spawnPoint.GetPosition(),
            currentTeam = team
        };

        var initData = new[]
        {
            (object) syncData
        };

        var spawnedObject = PhotonNetwork.Instantiate(_netModel.name,
            spawnPoint.GetPosition(), Quaternion.identity, 0, initData);
        spawnedObject.transform.SetParent(_spawnPointController.transform);
        _spawnPointController.TakeSpawnPoint(spawnPoint);

        var newUnit = spawnedObject.GetComponent<BaseUnitControllerNet>();
        newUnit.Init(hero._heroModel, team, hero._heroLevel, team == CurrentTeam);
        newUnit.name = hero._heroModel.CharacterName;

        SendModelDataToNet(syncData);

        _playerTeam.Add(new SpawnedUnit
        {
            ButtonID = buttonID,
            unitController = newUnit,
            SpawnPoint = spawnPoint
        });
    }

    private void SendModelDataToNet(SyncData data)
    {
        var riseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.MasterClient
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent((byte) NetEvents.SpawnUnit, data, riseEventOptions, sendOptions);
    }

    private void InstantiateReceivedModal(SyncData data)
    {
        /*FindObjectsOfType<BaseUnitController>().ToList().Find(item=> item.)*/
        /*var currentModel = _models.GetModelByName(data.modalName);
        
        var spawnedObject = PhotonNetwork.Instantiate(_netModel.name,
            data.spawnPos, Quaternion.identity);

        spawnedObject.transform.SetParent(_spawnPointController.transform);
        var newUnit = spawnedObject.GetComponent<BaseUnitController>();
        newUnit.Init(currentModel, EnemyTeam, data.heroLevel, false);
        newUnit.name = currentModel.CharacterName;

        _enemyTeam.Add(newUnit);*/
    }

    public void AddEnemy(BaseUnitController enemy)
    {
        _enemyTeam.Add(enemy);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte) NetEvents.SpawnUnit:
            {
                InstantiateReceivedModal((SyncData) photonEvent.CustomData);
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