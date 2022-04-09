using UnityEngine;
using UnityEngine.UI;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private Image _viewImage;
    [SerializeField] private RectTransform _viewRectTransform;

    public void Setup(ProjectileModel _model)
    {
        SetSprite(_model.ViewSprite, _model.ViewSpriteScale);
    }


    private void SetSprite(Sprite sprite, Vector3 scale)
    {
        _viewImage.sprite = sprite;
        _viewRectTransform.localScale = scale;
    }
}