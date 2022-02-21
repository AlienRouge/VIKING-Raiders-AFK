using System.Collections.Generic;
using UnityEngine;

public class Map : ScriptableObject
{
    public List<PlayerSpawnPoint> PlayerSpawnPoints { get; private set; }
    public List<EnemiesSpawnPoint> EnemySpawnPoints { get; private set; }
}
