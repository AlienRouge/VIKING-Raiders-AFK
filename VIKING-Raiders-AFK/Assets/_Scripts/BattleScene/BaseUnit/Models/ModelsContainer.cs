using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Models container", menuName = "Models/container")]
public class ModelsContainer : ScriptableObject
{
    [SerializeField] private List<BaseUnitModel> _models;
    
    private static ModelsContainer _instance;

    public static ModelsContainer Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(nameof(SpawnController) + "is NULL!");

            return _instance;
        }
    }

    public void Init()
    {
        _instance = this;
    }

    public BaseUnitModel GetModelByName(string modelName)
    {
        return _models.Find(model => model.name == modelName);
    }
}
