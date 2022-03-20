using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Scripts;

public class TownLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogUI.Instance.SetTitle("Town").SetMessage("This town is created by very old man...").Show();
    }
}
