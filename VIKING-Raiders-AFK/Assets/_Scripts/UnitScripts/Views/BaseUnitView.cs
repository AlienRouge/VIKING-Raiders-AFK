using _Scripts.Enums;
using UnityEngine;

public class BaseUnitView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private HealthBar _healthBar;


    public void SetTeamColor(Team team)
    {
        _spriteRenderer.color = (team == Team.Team1)
            ? new Color(53 / 255f, 72 / 255f, 231 / 255f)
            : new Color(250 / 255f, 52 / 255f, 37 / 255f);
    }
    
    private void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        _spriteRenderer.sprite = sprite;
        transform.localScale = scale;
    }

    public void OnTakeDamage(float health)
    {
        _healthBar.SetHealth(health);
    }
    

    public void Init(BaseUnitModel model)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthBar = GetComponentInChildren<HealthBar>();
        
        SetUnitSprite(model.viewSprite, model.spriteScale);
        _healthBar.SetMaxHealth(model.baseHealth);
    }
}