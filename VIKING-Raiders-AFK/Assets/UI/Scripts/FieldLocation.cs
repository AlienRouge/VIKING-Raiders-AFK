using UnityEngine;

namespace UI.Scripts
{
    public class FieldLocation : MonoBehaviour
    {
        void Start()
        {
            DialogUI.Instance.SetTitle("Field").SetMessage("This field is very pretty and calm...").Show();
        }
    }
}
