using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideDraftPanelButtonView : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private HeroDraftController _heroDraftController;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _default, _pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _heroDraftController.SwitchPanelHideStatus();
        _image.sprite = _heroDraftController.IsHidden ? _pressed : _default;
    }
}