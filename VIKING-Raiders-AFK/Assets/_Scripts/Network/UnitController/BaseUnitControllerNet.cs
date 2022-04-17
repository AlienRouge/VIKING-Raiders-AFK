using _Scripts.Network.SyncData;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.Network.UnitController
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class BaseUnitControllerNet : BaseUnitController, IPunObservable, IPunInstantiateMagicCallback, IOnEventCallback
    {
        [SerializeField] private PhotonView _photonView;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            /*if (stream.IsWriting)
            {
                stream.SendNext(ActualStats.CurrentHealth);
            }

            if (stream.IsReading)
            {
                ActualStats.CurrentHealth = (float) stream.ReceiveNext();
                EventController.UnitHealthChanged.Invoke(this, ActualStats.CurrentHealth);
            }*/
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (info.Sender.IsLocal) return;
            var initData = (SyncData.SyncData) info.photonView.InstantiationData[0];
            var model = ModelsContainer.Instance.GetModelByName(initData.modalName);
            Init(model, initData.currentTeam, initData.heroLevel, false);
            SpawnControllerNet.Instance.AddEnemy(this);
        }
        
        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case (byte)NetEvents.UnitHit:
                    var data = (SyncDamageData) photonEvent.CustomData;

                    if (_photonView.ViewID == data.ViewId)
                    {
                        ChangeHealth(data.Damage);
                    }
                    break;
                case (byte)NetEvents.UnitUseAbility:
                    var dataAbility = (SyncUseAbility) photonEvent.CustomData;

                    if (_photonView.ViewID == dataAbility.ViewId)
                    {
                        base.UseActiveAbility();
                    }
                    break;
                default:
                    break;
            }
        }
        
        protected override void UseActiveAbility()
        {
            SendHitData(new SyncUseAbility
            {
                ViewId = GetComponent<PhotonView>().ViewID
            });
            base.UseActiveAbility();
        }
        
        private void SendHitData(SyncUseAbility data)
        {
            var riseEventOptions = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.Others
            };
            var sendOptions = new SendOptions()
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent((byte) NetEvents.UnitUseAbility, data, riseEventOptions, sendOptions);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.AddCallbackTarget(this);
        }
    }
}