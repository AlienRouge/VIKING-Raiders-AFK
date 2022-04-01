using Photon.Pun;
using UnityEngine;

namespace _Scripts.Network.UnitController
{
    [RequireComponent(typeof(PhotonView))]
    public class BaseUnitControllerNet : BaseUnitController, IPunObservable, IPunInstantiateMagicCallback
    {
        private PhotonView _photonView;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            /*if (stream.IsWriting)
            {
                stream.SendNext();
            }
            else
            {
                stream.ReceiveNext();
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
    }
}