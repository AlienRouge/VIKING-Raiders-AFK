using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Range/Unit")]
public class BaseRangeUnitModelModel : BaseUnitModelModel
{
    [SerializeField] protected MagicalDamageType magicalDamageMeleeType;
}