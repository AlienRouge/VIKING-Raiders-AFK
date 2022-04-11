using UnityEngine;
using UnityEngine.UI;

public class BaseUnitView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _unitView;

    private Image _viewImage;
    private RectTransform _viewRectTransform;
    private BaseUnitController _parent;

    public void Init(BaseUnitController parent)
    {
        _parent = parent;
        _viewImage = _unitView.GetComponent<Image>();
        _viewRectTransform = _unitView.GetComponent<RectTransform>();

        SetUnitSprite(parent.ActualStats.Model.ViewSprite, parent.ActualStats.Model.ViewSpriteScale);
        _healthBar.SetMaxHealth(parent.ActualStats.Model.BaseHealth);
    }

    private void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        _viewImage.sprite = sprite;
        _viewRectTransform.localScale = scale;
    }

    // TODO Сомнительно?
    private void OnChangeHealth(BaseUnitController unit ,float health)
    {
        if (ReferenceEquals(unit, _parent))
        {
            _healthBar.SetHealth(health);
        }
    }
    
    private void OnEnable()
    {
        EventController.UnitHealthChanged += OnChangeHealth;
    }

    private void OnDisable()
    {
        EventController.UnitHealthChanged -= OnChangeHealth;
    }
}