using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.UnitScripts;
using _Scripts.UnitScripts.Views;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<Team, BaseUnitView> UnitDied;
    public static UnityAction GameEnded;
    
    /*public static UnityAction<BaseUnitView> UnitsSpawned;*/
}
