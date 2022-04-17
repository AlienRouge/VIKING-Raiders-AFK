using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleLevelCreator : MonoBehaviour
{
    [SerializeField] private User _playerData;
    [SerializeField] private ModelsContainer _modelsContainer;

    public BattleLevelModel GenerateBattleLevel()
    {
        var newBattleLevel = ScriptableObject.CreateInstance<BattleLevelModel>();
        newBattleLevel.Generated = true;
        newBattleLevel.EnemyHeroes = GenerateEnemyList();
        MapGenerator.Seed = Random.Range(int.MinValue, int.MaxValue);


        return newBattleLevel;
    }

    private List<Hero> GenerateEnemyList()
    {
        var playerUnitLevelSum = _playerData.heroList.Sum(hero => hero._heroLevel);
        int a = playerUnitLevelSum;
        List<Hero> enemyList = new List<Hero>();


        while (playerUnitLevelSum > 0 && enemyList.Count < Consts.MAX_PLAYER_TEAM_SIZE)
        {
            int maxLevel = playerUnitLevelSum < 20 ? playerUnitLevelSum : 20;
            int level = Random.Range(1, maxLevel);

            var hero = new Hero
            {
                _heroLevel = level,
                _heroModel = _modelsContainer.GetRandomModel(),
            };

            enemyList.Add(hero);
            playerUnitLevelSum -= level;
        }

        return enemyList;
    }
}