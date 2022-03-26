using UnityEngine;
using _Scripts.Network.Map;

public class BattleSceneControllerNet : BattleSceneController
{
    [SerializeField] private MapGeneratorNet mapGenerator;
    private void Start()
    {
        _instance = this;
        SetSpawnController();
    }
    protected override void SetSpawnController()
    {
        _spawnController = SpawnControllerNet.Instance;
    }

    public void InitScene()
    {
        _mapController = mapGenerator.GenerateMap();
        InitializeScene(_player, _enemy, _mapController);
    }
}