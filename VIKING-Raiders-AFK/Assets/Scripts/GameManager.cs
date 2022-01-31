using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private List<BaseUnit> allUnitPrefabs;
    private Dictionary<Team, List<BaseUnit>> unitsByTeams = new Dictionary<Team, List<BaseUnit>>();
    [SerializeField] private int unitsPerTeam = 2;

    void Start()
    {
        enemySpawnPoints = FindObjectOfType<EnemySpawnPoint>().GetComponentsInChildren<Transform>();
        playerSpawnPoints = FindObjectOfType<PlayerSpawnPoint>().GetComponentsInChildren<Transform>();
       
        InstantiateUnits();
        // Create 2 teams
    }

    private void InstantiateUnits()
    {
        unitsByTeams.Add(Team.Team1, new List<BaseUnit>());
        unitsByTeams.Add(Team.Team2, new List<BaseUnit>());
        for (int i = 0; i < unitsPerTeam; i++)
        {
            // Units for Team 1
            int randomUnitIndex = Random.Range(0, allUnitPrefabs.Count);
            BaseUnit newUnit = Instantiate(allUnitPrefabs[randomUnitIndex]);
            unitsByTeams[Team.Team1].Add(newUnit);

            int randomSpawnIndex = Random.Range(1, playerSpawnPoints.Length);
            newUnit.Init(Team.Team1, playerSpawnPoints[randomSpawnIndex]);

            // Units for Team 2
            randomUnitIndex = Random.Range(0, allUnitPrefabs.Count);
            newUnit = Instantiate(allUnitPrefabs[randomUnitIndex]);
            unitsByTeams[Team.Team2].Add(newUnit);

            randomSpawnIndex = Random.Range(1, enemySpawnPoints.Length);
            newUnit.Init(Team.Team2, enemySpawnPoints[randomSpawnIndex]);
        }
    }

    public enum Team
    {
        Team1,
        Team2
    }
}