using System;
using _Scripts.Enums;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    private static BattleSceneController _instance;

    protected User _playerData;

    [field: SerializeField] public BattleController BattleController { get; private set; }
    [field: SerializeField] public SpawnController SpawnController { get; private set; }
    [field: SerializeField] public UIController UIController { get; private set; }

    [SerializeField] protected MapGenerator _mapGenerator;
    protected MapController _mapController;

    [SerializeField] private GameLevelModel _gameLevel;

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

        _playerData = Resources.Load<User>("Player");
        
        _mapController = _playerData.currentGameLevel.Generated
            ? _mapGenerator.GenerateMap()
            : InstantiateMap(_gameLevel);
        
        InitializeScene(_playerData);
    }

    private MapController InstantiateMap(GameLevelModel levelModel)
    {
        MapController newMap = Instantiate(levelModel.LevelPrefab);
        newMap.BakeMap();

        return newMap;
    }


    private void InitializeScene(User playerData)
    {
        UIController.Init(playerData.heroList);
        SpawnController.Init(_mapController.SpawnPointsController);
        SpawnController.SpawnEnemies(_playerData.currentGameLevel.EnemyHeroes);
    }

    // Start button or smth else
    public virtual void OnStartButtonHandler()
    {
        if (SpawnController.PlayerTeamSize <= 0 || SpawnController.EnemyTeamSize <= 0) return;

        EventController.BattleStarted?.Invoke();
        UIController.Show_BP(SpawnController.GetPlayerUnits());
        BattleController.StartBattle(SpawnController.GetSpawnedUnits());
    }

    private void OnBattleEnded()
    {
        var winnerTeam = BattleController.WinnerTeam;

        if (winnerTeam != null)
        {
            Debug.Log("WINNER: " + winnerTeam);
        }
        else
        {
            Debug.Log("DRAW");
        }
    }

    private void OnEnable()
    {
        EventController.BattleEnded += OnBattleEnded;
    }

    private void OnDisable()
    {
        EventController.BattleEnded -= OnBattleEnded;
    }
}