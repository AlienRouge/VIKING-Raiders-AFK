using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/Magic")]
public class MagicAbility : BaseAbility
{
   [SerializeField] private MagicalDamageType _magicalDamageType;
   [SerializeField] private float baseDamage;

   public override void OnActivate(BaseUnitController target)
   {
   }
}
