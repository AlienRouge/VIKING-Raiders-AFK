using UnityEngine;

namespace UI.Scripts
{
    public class CastleLocation : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DialogUI.Instance.SetTitle("Castle").SetMessage("This castle is very strange and horrible place with monsters!..").Show();
        }
    }
}
