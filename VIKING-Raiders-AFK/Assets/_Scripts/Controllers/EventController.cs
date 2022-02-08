using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<Team, BaseUnitController> UnitDied;
    public static UnityAction GameEnded;
}
