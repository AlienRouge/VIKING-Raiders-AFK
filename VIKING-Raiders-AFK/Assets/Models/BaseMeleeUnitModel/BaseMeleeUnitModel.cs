using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Base/melee/unit")]
public class BaseMeleeUnitModel : BaseUnitModel
{
    [SerializeField] private float additionalArmor; //Щит
}
