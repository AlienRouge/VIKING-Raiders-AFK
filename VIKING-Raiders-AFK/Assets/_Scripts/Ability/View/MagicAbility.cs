using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Interfaces;
using _Scripts.UnitScripts.Views;
using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Abilities/Magic")]

public class MagicAbility : Ability, IMagicAbility
{
    [SerializeField] private MagicalDamageType _damageType;
    public MagicalDamageType damageType => _damageType;

    private float _lastUse;

    public override void Init()
    {
        _lastUse = 0;
    }
    public override void Use(BaseUnitView target)
    {
        Debug.Log("dasda");
        if (CheckPossibilityToUseAbility())
        {
            Debug.Log($"I use ability on {target.characterName}");
            _lastUse = Time.time+castTime;
            target.TakeDamageFromAbility(damageType, currentDamage);
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
        Debug.Log(cooldown);
        Debug.Log(_lastUse);
        Debug.Log(_lastUse + cooldown - Time.time <= 0);
        Debug.Log(_lastUse + cooldown );
        Debug.Log(Time.time);
       return _lastUse + cooldown - Time.time <= 0;
    }
}
