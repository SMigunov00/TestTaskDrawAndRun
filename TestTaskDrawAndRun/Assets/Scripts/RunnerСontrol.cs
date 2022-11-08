using UnityEngine;

[RequireComponent(typeof(GridLocation))]
public class RunnerСontrol : MonoBehaviour
{
    [SerializeField] private int _count;

    private DrawingConstructionWithMouse _drawer = null;
    private RunnerSpawner _runnerSpawner = null;

    public int Count => _count;

    private void Awake()
    {
        _drawer = FindObjectOfType<DrawingConstructionWithMouse>();
        _runnerSpawner = FindObjectOfType<RunnerSpawner>();
    }

    private void OnEnable()
    {
        _drawer.OnPointsGenerated += RecalculatePositions;
        _runnerSpawner.OnRunnerSpawned += OnRunnerSpawned;
    }

    private void OnDisable()
    {
        _drawer.OnPointsGenerated -= RecalculatePositions;
        _runnerSpawner.OnRunnerSpawned -= OnRunnerSpawned;
    }

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        GridLocation grid = GetComponent<GridLocation>();

        Vector3[] positions = grid.Calculate();

        for (int i = 0; i < positions.Length; i++)
            _runnerSpawner.SpawnAt(positions[i]);
    }

    private void OnRunnerSpawned(RunnerSpawner.OnRunnerSpawnedEventArgs args)
    {
        _count += 1;

        args.Runner.OnSpikeCollision += OnRunnerSpikeCollision;
    }

    private void OnRunnerSpikeCollision(RunnerObject.OnSpikeCollisionEventArgs args)
    {
        args.Sender.OnSpikeCollision -= OnRunnerSpikeCollision;

        _count -= 1;

        if (_count == 0)
            FindObjectOfType<GameState>().Died();
    }

    private void RecalculatePositions()
    {
        _runnerSpawner.RecountPosition(_count);
    }
}
