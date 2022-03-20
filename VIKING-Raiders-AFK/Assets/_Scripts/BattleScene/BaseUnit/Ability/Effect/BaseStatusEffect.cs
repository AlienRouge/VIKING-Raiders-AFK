using System;
using UnityEngine;
[Serializable]
public class BaseStatusEffect
{
    private string _name;
    private string _description;
    private int _ID;
    private int _applyPercentage;
    
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    public string Description
    {
        get => _description;
        set => _description = value;
    }
    
    public int ID
    {
        get => _ID;
        set => _ID = value;
    }
    
    public int ApplyPercentage
    {
        get => _applyPercentage;
        set => _applyPercentage = value;
    }
}
