using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectile")]
public class ProjectileModel : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite ViewSprite { get; private set; }
    [field: SerializeField] public Vector3 ViewSpriteScale { get; private set; }
    
    [field: SerializeField] public Vector3 ViewRotate { get; private set; }
    [field: SerializeField] public float FlightSpeed { get; private set; }
    [field: SerializeField] public BaseStatusEffect StatusEffect { get; private set; }
}
