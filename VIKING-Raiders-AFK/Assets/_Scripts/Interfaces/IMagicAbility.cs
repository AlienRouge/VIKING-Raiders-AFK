using _Scripts.Enums;

namespace _Scripts.Interfaces
{
    public interface IMagicAbility: IAbility
    {
        public MagicalDamageType damageType { get; }
    }
}