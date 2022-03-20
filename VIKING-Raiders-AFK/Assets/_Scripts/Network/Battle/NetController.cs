using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetController : MonoBehaviourPunCallbacks
{
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
        Debug.Log($"player {newPlayer.NickName} joined to game");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"player {otherPlayer.NickName} left the game");
    }
}
