using UnityEngine;

[CreateAssetMenu(fileName = "Base melee unit", menuName = "Units/Base/Melee/Unit")]
public class BaseMeleeUnitModel : BaseUnitModel
{
    [field: SerializeField] public int BaseMeleeDamage { get; private set; }


    public override ProjectileModel GetProjModel()
    {
        return null;
    }

    public override int GetUnitDamage(int unitLevel)
    {
        return (int)Mathf.Floor(BaseMeleeDamage * (1f + unitLevel / (float)MaxUnitLevel));
    }
}
