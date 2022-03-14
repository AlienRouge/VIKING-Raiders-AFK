using _Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera _camera;

    private Vector3 UnitPositon => GetMousePos() - _offset;
    private Vector2 _offset;
    private Vector2 _originalPos;

    private void Start()
    {
        _camera = Camera.main;
        _originalPos = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _offset = GetMousePos() - (Vector2)transform.position;
        // Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EventController.unitDrag.Invoke(Team.Team1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = UnitPositon;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventController.unitDrag.Invoke(Team.Team1);
        
        Vector2 mousePos = GetMousePos();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit)
        {
            if (hit.transform.TryGetComponent(out SpawnArea spawnArea))
            {
                _originalPos = UnitPositon;
            }
        }
        
        transform.position = _originalPos;
    }

    private Vector2 GetMousePos()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
