using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Image _background;

    [SerializeField] private Color _cooldownReady;
    [SerializeField] private Gradient _cooldownGradient;
    [SerializeField] private Color _deadColor;

    private float _cooldownTime;
    private bool _isCooldown;

    public void SetupItemView(BaseUnitModel model)
    {
        _title.text = model.CharacterName;
        _icon.sprite = model.ViewSprite;
        
        if (model.ActiveAbility)
        {
            _cooldownTime = model.ActiveAbility.CooldownTime;
            _cooldownImage.enabled = true;
        }
        _cooldownImage.pixelsPerUnitMultiplier = 4f;
    }

    public void OnAbilityReady()
    {
        _cooldownImage.fillAmount = 1f;
        _cooldownImage.color = _cooldownReady;
    }

    public void OnAbilityCooldown()
    {
        _cooldownImage.fillAmount = 0f;
        _isCooldown = true;
    }

    public void UnitDied()
    {
        _cooldownImage.enabled = false;
        _background.color = _deadColor;
    }

    private void Update()
    {
        if (!_isCooldown) return;
        _cooldownImage.fillAmount += 1f / _cooldownTime * Time.deltaTime;
        _cooldownImage.color = _cooldownGradient.Evaluate(_cooldownImage.fillAmount);
        
        if (!(_cooldownImage.fillAmount >= 1f)) return;
        _cooldownImage.fillAmount = 1f;
        _isCooldown = false;
    }
}
