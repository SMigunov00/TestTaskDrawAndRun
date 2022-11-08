using UnityEngine;

public class SpikeGroup : MonoBehaviour
{
    [SerializeField] private bool _isActive = false;

    [SerializeField] private SpikeObject _spikePrefab = null;
    [SerializeField] private SpikeObject[] _spikes = null;

    public bool IsActive => _isActive;

    private void Start()
    {
        SpawnGrid();
    }

    private void SpawnGrid()
    {
        GridLocation grid = GetComponent<GridLocation>();

        GetComponent<BoxCollider>().size = grid.TotalSize();

        Vector3[] positions = grid.Calculate();

        _spikes = new SpikeObject[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            SpikeObject spike = Instantiate(_spikePrefab, transform);
            spike.transform.localPosition = positions[i];
            _spikes[i] = spike;
        }

        UpdateState();
    }

    private void UpdateState()
    {
        for (int i = 0; i < _spikes.Length; i++)
            _spikes[i].UpdateState(_isActive);
    }

    private void OnValidate()
    {
        UpdateState();
    }
}