using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User data", menuName = "User/User")]
public class User : ScriptableObject
{
    [SerializeField] private string _userName;
    [SerializeField] private int _accountLevel;
    [SerializeField] private int _accountExp;
    [SerializeField] public List<Hero> _heroList;
    
    [Serializable]
    public struct Hero
    {
        public int _heroLevel;
        public BaseUnitModel _heroModel;
    }
}


