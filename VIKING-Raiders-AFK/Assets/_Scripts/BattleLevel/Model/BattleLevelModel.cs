using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New game level", menuName = "Game level")]
public class BattleLevelModel : ScriptableObject
{
    public bool Generated;
    
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public MapController MapPrefab { get; private set; }
    [field: SerializeField] public List<Hero> EnemyHeroes { get; set; }
    
}
