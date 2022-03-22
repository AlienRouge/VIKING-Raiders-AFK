using TMPro;
using UnityEngine;

public class HeroItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;

    public void SetupItemView(BaseUnitController unit)
    {
        _name.text = unit.characterName;
    }
}
