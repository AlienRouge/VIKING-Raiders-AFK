using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Range/Unit")]
public class BaseRangeUnitModel : BaseUnitModel
{
    [SerializeField] protected MagicalDamageType magicalDamageMeleeType;
}