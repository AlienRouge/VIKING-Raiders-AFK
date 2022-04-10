using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Melee/Unit")]
public class BaseMeleeUnitModel : BaseUnitModel
{
    public override ProjectileModel GetProjModel()
    {
        return null;
    }
}
