using _Scripts.Network.SyncData;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.Network.UnitController
{
    public class ProjectileControllerNet : ProjectileController
    {
        protected override void DoOnImpact()
        {
            var data = new SyncDamageData
            {
                ViewId = _target.GetComponent<PhotonView>().ViewID,
                Damage = -_damage
            };
        
            SendHitData(data);
            base.DoOnImpact();
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
}