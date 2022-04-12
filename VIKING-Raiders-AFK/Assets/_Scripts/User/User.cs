using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User data", menuName = "User/User")]
public class User : ScriptableObject
{
    [SerializeField] public string _userName;
    [SerializeField] public int _accountLevel;
    [SerializeField] public int _accountExp;
    
    [Header("Hero pool")]
    [SerializeField] public List<Hero> heroList;
    
    [Header("GameLevelModel for battle scene")]
    [SerializeField] public GameLevelModel currentGameLevel;
}


