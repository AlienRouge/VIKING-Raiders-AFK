using UnityEngine;

public class SceneControllerNet : SceneController
{
    protected override void SetSpawnController()
    {
        _spawnController = SpawnControllerNet.Instance;
    }
}