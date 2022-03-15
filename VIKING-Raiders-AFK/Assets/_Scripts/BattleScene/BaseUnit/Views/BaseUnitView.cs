using _Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnitView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _unitView;

    private Image _viewImage;
    private RectTransform _viewRectTransform;

    private void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        _viewImage.sprite = sprite;
        _viewRectTransform.localScale = scale;
    }

    public void OnTakeDamage(float health)
    {
        _healthBar.SetHealth(health);
    }
    
    public void Init(BaseUnitModel model)
    {
        _viewImage = _unitView.GetComponent<Image>();
        _viewRectTransform = _unitView.GetComponent<RectTransform>();
        
        SetUnitSprite(model.ViewSprite, model.ViewSpriteScale);
        _healthBar.SetMaxHealth(model.BaseHealth);
    }
}