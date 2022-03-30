using UnityEngine;
using UnityEngine.UI;

// TODO REWORK
public class BaseUnitView : MonoBehaviour
{
    private BaseUnitController _parent;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _unitView;

    private Image _viewImage;
    private RectTransform _viewRectTransform;

    private void OnEnable()
    {
        EventController.UnitHealthChanged += OnChangeHealth;
    }

    private void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        _viewImage.sprite = sprite;
        _viewRectTransform.localScale = scale;
    }

    // TODO Сомнительно?
    private void OnChangeHealth(BaseUnitController unit ,float health)
    {
        Debug.Log("VAR");
        if (ReferenceEquals(unit, _parent))
        {
            _healthBar.SetHealth(health);
        }
    }

    public void Init(BaseUnitController parent)
    {
        _parent = parent;
        _viewImage = _unitView.GetComponent<Image>();
        _viewRectTransform = _unitView.GetComponent<RectTransform>();

        SetUnitSprite(parent.ActualStats.UnitModel.ViewSprite, parent.ActualStats.UnitModel.ViewSpriteScale);
        _healthBar.SetMaxHealth(parent.ActualStats.UnitModel.BaseHealth);
    }
}