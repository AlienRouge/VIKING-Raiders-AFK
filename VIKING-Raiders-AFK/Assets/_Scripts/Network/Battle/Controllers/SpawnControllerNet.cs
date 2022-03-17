using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;

public class SpawnControllerNet : SpawnContoller
{
    private void Start()
    {
        SpawnUnit = InstantiateUnitNet;
    }
    
    private void InstantiateUnitNet(Team team, User.Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        var spawnedObject = PhotonNetwork.Instantiate(_baseUnitPrefab.name,
            spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);
        var newUnit = spawnedObject.GetComponent<BaseUnitController>();
        newUnit.Init(hero._heroModel, team,  hero._heroLevel, team==Team.Team1);
        newUnit.name = hero._heroModel.CharacterName;

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
