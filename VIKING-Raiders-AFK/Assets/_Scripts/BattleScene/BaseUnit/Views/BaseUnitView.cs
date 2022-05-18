using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnitView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _unitView;

    private Image _viewImage;
    private RectTransform _viewRectTransform;
    private BaseUnitController _parent;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    public void Init(BaseUnitController parent)
    {
        _parent = parent;
        _viewImage = _unitView.GetComponent<Image>();
        _spriteRenderer = _unitView.GetComponent<SpriteRenderer>();
        _viewRectTransform = _unitView.GetComponent<RectTransform>();
        _animator = _unitView.GetComponent<Animator>();
        SetUnitSprite(parent.ActualStats.Model.ViewSprite, parent.ActualStats.Model.ViewSpriteScale);
        _healthBar.SetMaxHealth(parent.ActualStats.Model.BaseHealth);
    }

    public Animator SetAnimator(AnimatorController animatorController)
    {
        _animator.runtimeAnimatorController = animatorController;
        _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        _animator.updateMode = AnimatorUpdateMode.Normal;
        return _animator;
    }
    private void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        if (_viewImage != null)
        {
            _viewImage.sprite = sprite;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = sprite;
        }

        if (_viewRectTransform != null)
        {
            _viewRectTransform.localScale = scale;
        }
        else
        {
            _unitView.transform.localScale = scale;
           // transform.localScale = scale;
        }
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