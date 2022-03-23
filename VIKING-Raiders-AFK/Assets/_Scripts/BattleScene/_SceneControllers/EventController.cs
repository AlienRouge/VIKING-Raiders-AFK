using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<BaseUnitController> UnitDied;
    public static UnityAction BattleStarted;
    public static UnityAction BattleEnded;
    public static UnityAction<Team> UnitDragged;
    public static UnityAction<BaseUnitController> UseUnitActiveAbility;
    public static UnityAction<BaseUnitController, AbilityController.AbilityState> ActiveAbilityStateChanged;
    public static UnityAction<BaseUnitController, AbilityController.AbilityState> PassiveAbilityStateChanged;
}
// TODO Объединить active и passive