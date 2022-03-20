using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User data", menuName = "User/User")]
public class User : ScriptableObject
{
    [SerializeField] public string _userName;
    [SerializeField] public int _accountLevel;
    [SerializeField] public int _accountExp;
    [SerializeField] public List<Hero> _heroList;
    
    [Serializable]
    public struct Hero
    {
        public int _heroLevel;
        public BaseUnitModel _heroModel;
    }
}


