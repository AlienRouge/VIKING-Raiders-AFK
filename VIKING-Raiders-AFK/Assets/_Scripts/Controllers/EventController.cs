using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnitScripts;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<Team, BaseUnit> UnitDied;
    public static UnityAction GameEnded;
}
