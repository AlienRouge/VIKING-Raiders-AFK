using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;

public class SpawnControllerNet : SpawnController
{
    private void Start()
    {
        SpawnUnit = InstantiateUnitNet;
    }
    
    private void InstantiateUnitNet(Team team, BaseUnitModel unitModel, SpawnPoint spawnPoint, int buttonID = -1)
    {
        var spawnedObject = PhotonNetwork.Instantiate(_baseUnitPrefab.name,
            spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);
        var newUnit = spawnedObject.GetComponent<BaseUnitController>();
        newUnit.Init(unitModel, team, team==Team.Team1);
        /*newUnit.Init(unitModel, team, team==Team.Team1);*/
        newUnit.name = unitModel.characterName;

        if (team == Team.Team1)
        {
            _playerTeam.Add(new SpawnedUnit
            {
                ButtonID = buttonID, 
                unitController = newUnit, 
                SpawnPoint = spawnPoint
            });
        }
        else
        {
            _enemyTeam.Add(newUnit);
        }
    }
}
