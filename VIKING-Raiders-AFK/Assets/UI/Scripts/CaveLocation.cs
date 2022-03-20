using UnityEngine;

namespace UI.Scripts
{
    public class CaveLocation : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DialogUI.Instance.SetTitle("Cave").SetMessage("Nobody knows what kind of monsters there...").Show();
        }
    }
}
