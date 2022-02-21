using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public List<Transform> GetSpawnTransforms()
    {
        var transforms = GetComponentsInChildren<Transform>();
        return transforms.Skip(1).ToList();
    }
}
