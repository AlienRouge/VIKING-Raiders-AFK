using _Scripts.Enums;
using Photon.Pun;
using UnityEngine;

public class SpawnControllerNet : SpawnController
{
    private void Start()
    {
        SpawnUnit = InstantiateUnitNet;
        if (PhotonNetwork.IsMasterClient)
        {
            _currentTeam = Team.Team1;
        }
        else
        {
            _currentTeam = Team.Team2;
        }
    }

    private void InstantiateUnitNet(Team team, User.Hero hero, SpawnPoint spawnPoint, int buttonID = -1)
    {
        var spawnedObject = PhotonNetwork.Instantiate(_baseUnitPrefab.name,
            spawnPoint.GetPosition(), Quaternion.identity);
        _spawnPointController.TakeSpawnPoint(spawnPoint);
        
        var newUnit = spawnedObject.GetComponent<BaseUnitController>();
        newUnit.Init(hero._heroModel, team, hero._heroLevel, team == _currentTeam);
        newUnit.name = hero._heroModel.CharacterName;
        newUnit.transform.SetParent(_spawnPointController.transform);
        
        if (team == _currentTeam)
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