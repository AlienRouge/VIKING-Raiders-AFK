using UnityEngine;

namespace UI.Scripts
{
    public class BridgeLocation : MonoBehaviour
    {
        void Start()
        {
            DialogUI.Instance.SetTitle("Bridge").SetMessage("This bridge leads to the horrible castle...").Show();
        }
    }
}
