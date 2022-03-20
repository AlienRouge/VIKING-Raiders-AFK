using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class III : IPunPrefabPool
{
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }

    public void Destroy(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }
}

public class LobbyController : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.NickName = $"player {Random.Range(0, int.MaxValue)}";
        Debug.Log($"name was set {PhotonNetwork.NickName}");
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions()
        {
            MaxPlayers = 2,
        });
    }

    public void JoinRoom()
    {
        /*PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions()
        {
            MaxPlayers = 2,
        });*/
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("connected to room");
        
        PhotonNetwork.LoadLevel("NetBattleScene");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected to master");
    }
}
