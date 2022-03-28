using System;
using _Scripts.Network.SyncData;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetController : MonoBehaviourPunCallbacks
{
    [SerializeField] private BattleSceneControllerNet _battleSceneController;

    private void Start()
    {
        PhotonPeer.RegisterType(typeof(SyncData), 240, SyncData.Serialize, SyncData.Deserialize);
    }

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
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2) return;

        if (PhotonNetwork.IsMasterClient)
        {
            _battleSceneController.InitScene();
        }

        Debug.Log($"player {newPlayer.NickName} joined to game");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"player {otherPlayer.NickName} left the game");
    }
}
