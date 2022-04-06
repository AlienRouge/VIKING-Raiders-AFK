using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _default, _pressed;

    protected abstract void DoOnPointerDown();
    protected abstract void DoOnPointerUp();
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
        DoOnPointerDown();
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _default;
        DoOnPointerUp();
    }
}
