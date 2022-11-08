using UnityEngine;

public class GridLocation : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize = Vector2Int.one;
    [SerializeField] private Vector3 _normal = Vector3.up;
    [SerializeField] private float _chunkSize = 1;
    [SerializeField] private float _spacing = 1;

    public Vector3 TotalSize()
    {
        Vector3 size = Vector3.zero;
        size.x = _gridSize.x * _chunkSize + (_gridSize.x - 1) * _spacing;
        size.y = 1;
        size.z = _gridSize.y * _chunkSize + (_gridSize.y - 1) * _spacing;
        return size;
    }

    public Vector3[] Calculate()
    {
        Vector3[] positions = new Vector3[_gridSize.x * _gridSize.y];

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector3 position = EvaluateNormal(GetPosition(x, y));

                int index = y * _gridSize.x + x;
                positions[index] = position;
            }
        }

        return positions;
    }

    private Vector3 GetPosition(int x, int y)
    {
        Vector2 receivePosition = new Vector2(x, y);
        Vector2 centerOffset = Vector2.one * 0.5f;
        Vector2 gridOffset = new Vector2(-_gridSize.x * 0.5f, -_gridSize.y * 0.5f);
        Vector3 position = receivePosition + centerOffset + gridOffset;
        Vector3 newPosition = position * _chunkSize + position * _spacing;

        return newPosition;
    }

    private Vector3 EvaluateNormal(Vector3 xy)
    {
        return Quaternion.FromToRotation(Vector3.forward, _normal) * xy;
    }

    private Vector3[] GetCorners(Vector2 xy)
    {
        Vector3[] angles = new Vector3[4];

        Vector3 halfSize = Vector3.one * _chunkSize * 0.5f;

        angles[0] = EvaluateNormal(xy + new Vector2(-halfSize.x, -halfSize.y));
        angles[1] = EvaluateNormal(xy + new Vector2(-halfSize.x, halfSize.y));
        angles[2] = EvaluateNormal(xy + new Vector2(halfSize.x, halfSize.y));
        angles[3] = EvaluateNormal(xy + new Vector2(halfSize.x, -halfSize.y));

        return angles;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector3 xy = GetPosition(x, y);

                Vector3 pos = EvaluateNormal(xy);
                Gizmos.DrawWireSphere(pos + origin, 0.01f);

                var angles = GetCorners(xy);
                for (int i = 0; i < angles.Length; i++)
                    Gizmos.DrawLine(angles[i] + origin, angles[(i + 1) % angles.Length] + origin);
            }
        }
    }
}
