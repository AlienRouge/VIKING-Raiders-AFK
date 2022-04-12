using UnityEngine;
using UnityEngine.UI;

public class ModalMenuButtonView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _default, _pressed;
    
    public void SetPressedSprite()
    {
        _image.sprite = _pressed;
    }
    
    public void SetDefaultSprite()
    {
        _image.sprite = _default;
    }
}
