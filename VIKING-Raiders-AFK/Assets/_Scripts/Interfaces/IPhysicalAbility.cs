using _Scripts.Enums;

namespace _Scripts.Interfaces
{
    public interface IPhysicalAbility : IAbility
    {
        public PhysicalDamageType damageType { get; }
    }
}