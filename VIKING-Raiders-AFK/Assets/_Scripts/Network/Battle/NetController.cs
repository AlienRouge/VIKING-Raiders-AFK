using System;
using _Scripts.Network.SyncData;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private BattleSceneControllerNet _battleSceneController;

    private static bool isFirstPlayerReady = false;
    private static bool isSecondPlayerReady = false;
    
    private void Start()
    {
        PhotonPeer.RegisterType(typeof(SyncData), 240, Converter.Serialize, Converter.Deserialize);
        PhotonPeer.RegisterType(typeof(SyncDamageData), 241, Converter.Serialize, Converter.Deserialize);
        PhotonPeer.RegisterType(typeof(SyncNetAction), 242, Converter.Serialize, Converter.Deserialize);
        PhotonPeer.RegisterType(typeof(SyncReadyClick), 243, Converter.Serialize, Converter.Deserialize);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public bool ReadyClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isFirstPlayerReady = !isFirstPlayerReady;
            SendSyncReady(new SyncReadyClick
            {
                player = 1,
                isReady = isFirstPlayerReady
            }, NetEvents.ReadyClick);
        }
        else
        {
            isSecondPlayerReady = !isSecondPlayerReady;
            SendSyncReady(new SyncReadyClick
            {
                player = 2,
                isReady = isSecondPlayerReady
            }, NetEvents.ReadyClick);
        }

        return isFirstPlayerReady && isSecondPlayerReady;
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

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    private void SendSyncReady(SyncReadyClick data, NetEvents netEvent)
    {
        var riseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.Others
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent((byte) netEvent, data, riseEventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte) NetEvents.ReadyClick:
                var data = (SyncReadyClick) photonEvent.CustomData;

                if (data.player == 1)
                {
                    isFirstPlayerReady = data.isReady;
                }

                if (data.player == 2)
                {
                    isSecondPlayerReady = data.isReady;
                }
                break;
            default:
                break;
        }
    }
}
