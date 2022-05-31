using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<BaseUnitController> UnitDied;
    public static UnityAction<BaseUnitController, float> UnitHealthChanged;
    public static UnityAction BattleStarted;
    public static UnityAction BattleEnded;
    public static UnityAction<Team> UnitDragged;
    public static UnityAction<BaseUnitController> UseUnitActiveAbility;
    public static UnityAction<BaseUnitController, AbilityController.AbilityState> ActiveAbilityStateChanged;
    public static UnityAction<BaseUnitController, AbilityController.AbilityState> PassiveAbilityStateChanged;
        // public static UnityAction<Hero> HeroStatsFilling;
    public static UnityAction<BaseUnitModel, bool> HeroShopStatsFilling;
}
// TODO Объединить active и passive