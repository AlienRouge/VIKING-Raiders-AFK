using Photon.Pun;
using UnityEngine;

public class PanelControllerNet : PanelController
{
        protected override LayoutElement InstatiateLayoutElement()
        {
                var transform = _layoutElementPrefab.transform;
                return PhotonNetwork.Instantiate(_layoutElementPrefab.name, transform.position, transform.rotation).
                        GetComponent<LayoutElement>();
        }
}
