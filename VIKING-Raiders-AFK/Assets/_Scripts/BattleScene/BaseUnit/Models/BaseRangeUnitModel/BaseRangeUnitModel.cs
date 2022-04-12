using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Range/Unit")]
public class BaseRangeUnitModel : BaseUnitModel
{
    [field: SerializeField] public ProjectileModel ProjectileModel { get; private set; }

    public override ProjectileModel GetProjModel()
    {
        return ProjectileModel;
    }
}