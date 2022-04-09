using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Range/Unit")]
public class BaseRangeUnitModel : BaseUnitModel
{
    [field: SerializeField] public int BaseProjectileDamage { get; private set; }
    [field: SerializeField] public ProjectileModel ProjectileModel { get; private set; }

    public override ProjectileModel GetProjModel()
    {
        return ProjectileModel;
    }

    public override int GetUnitDamage(int unitLevel)
    {
        return (int)Mathf.Floor(BaseProjectileDamage * (1f + unitLevel / (float)MaxUnitLevel));
    }
}