using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface IUnitModel
    {
        public string characterName { get;}
        public string description { get;}
        public Sprite viewSprite { get;}
        public Vector3 spriteScale { get;}
        public float baseDamage { get;}
        public float baseHealth { get;}
        public float attackRange { get;}
        public float armor { get;}
        public UnitType unitType { get;} 
        public float moveSpeed { get;}
        public float attackSpeed { get;}
        public float damageMultiplier { get;}
        public PhysicalDamageType physicalDamageType { get;}
    }
}

