using System;
using _Scripts.Network.Map;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetController : MonoBehaviourPunCallbacks
{
    [SerializeField] private BattleSceneControllerNet _battleSceneController;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("enter");
            _battleSceneController.InitScene();
        }


        Debug.Log($"player {newPlayer.NickName} joined to game");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"player {otherPlayer.NickName} left the game");
    }
}
