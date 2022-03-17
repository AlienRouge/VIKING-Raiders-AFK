using Photon.Pun;
using UnityEngine;

public class PanelControllerNet : PanelController
{
    protected override LayoutElement InstantiateLayoutElement()
    {
        Debug.Log("");
        var transform = _layoutElementPrefab.transform;
        var currentElement =
            PhotonNetwork.Instantiate(_layoutElementPrefab.name, transform.position, transform.rotation);
        currentElement.transform.SetParent(_layoutGroup.transform);
        return currentElement.GetComponent<LayoutElement>();
    }
}