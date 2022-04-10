using _Scripts.Network.SyncData;
using _Scripts.Network.UnitController;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MeleeUnitControllerNet : BaseUnitControllerNet
{
    protected override void DoOnAttack(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        var data = new SyncDamageData
        {
            ViewId = _currentTarget.GetComponent<PhotonView>().ViewID,
            Damage = -damage
        };
        
        SendHitData(data);
        _currentTarget.ChangeHealth(-damage);

    }
    

    private void SendHitData(SyncDamageData data)
    {
        var riseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.Others
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent((byte) NetEvents.UnitHit, data, riseEventOptions, sendOptions);
    }
}