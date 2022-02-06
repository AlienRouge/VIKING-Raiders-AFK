using _Scripts.UnitScripts.Views;

namespace _Scripts.Interfaces
{
    public interface IAbility
    {
        public float cooldown { get; }
        public float powerModifier { get; }
        public float baseDamage { get; }
        public float castTime { get; }
        public void Use(BaseUnitView target);
        
        public bool CheckPossibilityToUseAbility();
    }
}
