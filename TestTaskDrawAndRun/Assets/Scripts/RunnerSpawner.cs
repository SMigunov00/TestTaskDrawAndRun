using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunnerSpawner : MonoBehaviour
{
    public event Action<OnRunnerSpawnedEventArgs> OnRunnerSpawned = null;
    public class OnRunnerSpawnedEventArgs
    {
        public RunnerObject Runner { get; }

        public OnRunnerSpawnedEventArgs(RunnerObject runner)
        {
            Runner = runner;
        }
    }

    [SerializeField] private Vector3 _runnersSpawnPosition = Vector3.zero;
    [SerializeField] private Vector2 _size = Vector2.one;
    [SerializeField] private RunnerObject _runnersPrefab = null;

    private Vector3[] _points = new Vector3[0];

    private List<RunnerObject> _runners = new List<RunnerObject>();

    private DrawingConstructionWithMouse _drawer = null;

    private void Awake()
    {
        _drawer = FindObjectOfType<DrawingConstructionWithMouse>();
    }

    private void Update()
    {
        var q = transform.rotation;
        _runnersSpawnPosition = q.eulerAngles;
        _runnersSpawnPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 size = new Vector3(_size.x, 0, _size.y);
        Gizmos.DrawWireCube(_runnersSpawnPosition, size);
    }

    public void RecountPosition(int count)
    {
        List<Vector3> points = _drawer.Points
            .Select(v2 => ConvertPoint(v2))
            .ToList();

        _points = new TrajectoryPath(points).Subdivide(count).ToArray();

        for (int i = 0; i < _runners.Count; i++)
        {
            RunnerObject runner = _runners[i];
            runner.transform.position = _points[i];
        }
    }

    public void SpawnAt(Vector3 position)
    {
        RunnerObject runner = Instantiate(_runnersPrefab, position, transform.rotation, transform);
        runner.OnSpikeCollision += OnSpikeRemoveRunner;
        _runners.Add(runner);
        OnRunnerSpawned?.Invoke(new OnRunnerSpawnedEventArgs(runner));
    }

    public void OnSpikeRemoveRunner(RunnerObject.OnSpikeCollisionEventArgs args)
    {
        args.Sender.OnSpikeCollision -= OnSpikeRemoveRunner;
        _runners.Remove(args.Sender);
        Destroy(args.Sender.gameObject);
    }

    private void GetCorners(Vector3[] angles)
    {
        Vector3 halfSize = _size * 0.5f;

        angles[0] = _runnersSpawnPosition + new Vector3(-halfSize.x, 0, -halfSize.y);
        angles[1] = _runnersSpawnPosition + new Vector3(-halfSize.x, 0, halfSize.y);
        angles[2] = _runnersSpawnPosition + new Vector3(halfSize.x, 0, halfSize.y);
        angles[3] = _runnersSpawnPosition + new Vector3(halfSize.x, 0, -halfSize.y);
    }

    private Vector3 ConvertPoint(Vector2 points)
    {
        Vector3[] angles = new Vector3[4];
        GetCorners(angles);

        Vector3 point = Vector3.zero;
        point.x = Mathf.Lerp(angles[0].x, angles[3].x, points.x);
        point.z = Mathf.Lerp(angles[0].z, angles[1].z, points.y);
        return point;
    }
}