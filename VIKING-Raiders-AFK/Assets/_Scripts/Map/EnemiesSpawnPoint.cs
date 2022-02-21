using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawnPoint : MonoBehaviour
{
    public List<Transform> GetSpawnTransforms()
    {
        var transforms = GetComponentsInChildren<Transform>();
        return transforms.Skip(1).ToList();
    }
}
