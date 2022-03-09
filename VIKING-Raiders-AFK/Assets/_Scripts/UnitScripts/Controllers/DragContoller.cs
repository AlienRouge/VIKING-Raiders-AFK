using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragContoller : MonoBehaviour
{
    private Camera _camera;
    private BoxCollider2D _boxCollider2D;
    
    private bool _dragging;
    private Vector2 _offset;
    private Vector2 _OriginalPos;
    private void Awake()
    {
        _camera = Camera.main;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _OriginalPos = transform.position;
    }

    private void Update()
    {
        if (!_dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - _offset;
    }

    private void OnMouseDown()
    {
        _dragging = true;
        _offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp()
    {
        // transform.position = _OriginalPos;
        _dragging = false;
    }

    private Vector2 GetMousePos()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Disable()
    {
        _boxCollider2D.enabled = false;
        enabled = false;
    }
    
    public void Enable()
    {
        _boxCollider2D.enabled = true;
        enabled = true;
    }
}
