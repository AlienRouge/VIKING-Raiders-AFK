using _Scripts.Enums;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
   public Team Team;
   public bool IsTaken;

   public Vector3 GetPosition()
   {
      return transform.position;
   }
}
