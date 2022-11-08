using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawingConstructionWithMouse : MonoBehaviour
{
    public event Action OnPointsGenerated = null;

    [SerializeField] private RectTransform _rectTransform = null;
    [SerializeField] private float _doorstepDistance = 5;

    private bool _isDrawing = false;

    private List<Vector2> _points = new List<Vector2>();
    public List<Vector2> Points => _points;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDrawing = true;
            _points.Clear();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDrawing = false;

            if (_points.Count >= 2)
                OnPointsGenerated?.Invoke();
        }

        if (_isDrawing)
            HandleDrawing(Input.mousePosition);
    }

    private Vector2 GetPosition(Vector2 mouseScreenPosition)
    {
        Vector3[] angles = new Vector3[4];
        _rectTransform.GetWorldCorners(angles);

        Vector2 horizontal = new Vector2(angles[0].x, angles[3].x);
        Vector2 vertical = new Vector2(angles[0].y, angles[1].y);

        Vector2 position01 = new Vector2(
            Mathf.InverseLerp(horizontal.x, horizontal.y, mouseScreenPosition.x),
            Mathf.InverseLerp(vertical.x, vertical.y, mouseScreenPosition.y)
        );

        return position01;
    }

    private void HandleDrawing(Vector2 mouseScreenPosition)
    {
        bool inInside = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mouseScreenPosition);

        if (!inInside)
            return;

        Vector3 position = GetPosition(mouseScreenPosition);

        if (_points.Count == 0)
            _points.Add(position);
        else
        {
            Vector3 last = _points[_points.Count - 1];

            if (Vector3.Distance(position, last) > _doorstepDistance * 0.01f)
                _points.Add(position);
        }
    }
}