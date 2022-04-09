using System.Threading.Tasks;
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
            if (stream.IsWriting)
            {
                stream.SendNext(ActualStats.CurrentHealth);
            }
            else
            {
                ActualStats.CurrentHealth = (float) stream.ReceiveNext();
                EventController.UnitHealthChanged.Invoke(this, ActualStats.CurrentHealth);
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (info.Sender.IsLocal) return;
            var initData = (SyncData.SyncData) info.photonView.InstantiationData[0];
            var model = ModelsContainer.Instance.GetModelByName(initData.modalName);
            Init(model, initData.currentTeam, initData.heroLevel, false);
            SpawnControllerNet.Instance.AddEnemy(this);
        }

        protected override async Task Attack()
        {
            if (ActualStats.IsDead || _isBattleEnd || _currentTarget.ActualStats.IsDead) return;

            var damage = CalculateDamage();
            Debug.Log(
                $"{ActualStats.UnitModel.CharacterName} --> {_currentTarget.ActualStats.UnitModel.CharacterName} [{damage}]dmg");
            _currentTarget.ChangeHealth(-damage);
            await Task.Delay(Mathf.RoundToInt(ActualStats.AttackDeltaTime * Consts.ONE_SECOND_VALUE));
        }

        protected override void DoOnAttack(int damage)
        {
            throw new System.NotImplementedException();
        }
    }
}