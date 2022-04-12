using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New game level", menuName = "Game level")]
public class GameLevelModel : ScriptableObject
{
    [field: SerializeField] public string LevelName { get; private set; }
    [field: SerializeField] public MapController LevelPrefab { get; private set; }
    public bool Generated;
    [field: SerializeField] public List<Hero> EnemyHeroes { get; private set; }
    
}
