using _Scripts.Enums;
using _Scripts.UnitScripts;
using UnityEngine;

public class BaseUnitView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;


    public void SetUnitSprite(Sprite sprite, Vector3 scale)
    {
        Debug.Log(_spriteRenderer);
        _spriteRenderer.sprite = sprite;
        transform.localScale = scale;
    }

    public void SetTeamColor(Team team)
    {
        _spriteRenderer.color = (team == Team.Team1)
            ? new Color(53/255f,72/255f,231/255f)
            : new Color(250/255f, 52/255f, 37/255f);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}