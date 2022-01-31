using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int baseDamage;
    [SerializeField] private int baseHealth;
    
    [Range(1, 5)] 
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float moveSpeed = 1f;
}
