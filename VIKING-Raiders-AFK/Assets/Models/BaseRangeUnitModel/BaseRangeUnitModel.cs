using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Base/Range/unit")]
public class BaseRangeUnitModel : BaseUnitModel
{
    [SerializeField] protected MagicalDamageType magicalDamageMeleeType;
}