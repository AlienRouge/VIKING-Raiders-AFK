using _Scripts.Network.Map;
using UnityEngine;

public class BattleSceneControllerNet : BattleSceneContoller
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
        Debug.Log("init scene"+1541);
        _mapController = mapGenerator.GenerateMap();
        InitializeScene(_player, _enemy, _mapController);
        Debug.Log(123);
    }
}