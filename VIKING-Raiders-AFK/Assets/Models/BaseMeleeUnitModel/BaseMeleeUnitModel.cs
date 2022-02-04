using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Melee/Unit")]
public class BaseMeleeUnitModel : BaseUnitModel
{
    [SerializeField] private float additionalArmor; //Щит
}
