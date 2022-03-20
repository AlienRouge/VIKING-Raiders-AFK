using UnityEngine;

public class BattleSceneControllerNet : BattleSceneContoller
{
    protected override void SetSpawnController()
    {
        _spawnController = SpawnControllerNet.Instance;
        Debug.Log(_spawnController);
    }
}