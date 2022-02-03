using System.Linq;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public Transform[] GetSpawnTransforms()
    {
        var transforms = GetComponentsInChildren<Transform>();
        return transforms.Skip(1).ToArray();
    }
}