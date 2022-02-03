using UnityEngine;

[CreateAssetMenu(fileName = "New hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    public string name;
    public string description;
    public int baseDamage;
    public int baseHealth = 1;
    
    [Range(1, 5)] 
    public float attackRange = 1f;
    public float attackSpeed = 1f;
    public float moveSpeed = 1f;
}
