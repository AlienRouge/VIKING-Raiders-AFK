using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Abilities/Magic")]

public class MagicAbility : Ability
{
    [SerializeField] private MagicalDamageType _damageType;
    public MagicalDamageType damageType => _damageType;

    private float _lastUse;

    public override void Init()
    {
        _lastUse = 0;
    }
    public async override Task Use(BaseUnitView target)
    {
        if (CheckPossibilityToUseAbility())
        {
            // TODO 
            // await Task.Delay(Mathf.RoundToInt(castTime * 1000));
            // Debug.Log($"I use ability on {target.characterName}");
            // _lastUse = Time.time+castTime;
            // target.TakeDamageFromAbility(damageType, currentDamage);
        }
    }
    
    public override bool CheckPossibilityToUseAbility()
    {
        return CheckDistance() && CheckCooldown();
    }

    private bool CheckDistance()
    {
        return true;
    }

    private bool CheckCooldown()
    {
        return _lastUse + cooldown - Time.time <= 0;
    }
}
