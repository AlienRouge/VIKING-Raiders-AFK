using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "Abilities/Magic")]
public class MagicAbility : BaseAbility
{
    [SerializeField] private MagicalDamageType _magicalDamageType;
    [SerializeField] private float baseDamage;

    public override async Task OnActivate(BaseUnitController target)
    {
        Debug.Log("Casting..." + AbilityName);
        await Task.Delay(Mathf.RoundToInt( CastTime * 1000));
        Debug.Log("Use ability. Piu! on " + target.characterName);
        target.TakeDamage(baseDamage);
    }

    public override void OnAbilityBeginCooldown()
    {
        Debug.Log("Begin Cooldown");
    }
}