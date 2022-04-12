using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public struct Hero
{
    [SerializeField, DefaultValue(1)] public int _heroLevel;
    [SerializeField] public BaseUnitModel _heroModel;
}