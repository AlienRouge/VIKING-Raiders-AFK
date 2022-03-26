using UnityEngine;

namespace UI.Scripts
{
    public class Spawn : MonoBehaviour
    {
        public void SpawnNeedObj(GameObject objPrefab)
        {
            Instantiate(objPrefab);
        }
    }
}
