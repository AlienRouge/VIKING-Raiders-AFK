using System;
using UnityEngine;

public class DragContoller : MonoBehaviour
{
    private Camera _camera;
    
    private bool _dragging;
    private Vector2 _offset;
    private Vector2 _OriginalPos;
    private void Start()
    {
        _camera = Camera.main;
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
}
