using System;
using UnityEngine;

public class RunnerObject : MonoBehaviour
{
    public class OnSpikeCollisionEventArgs
    {
        public RunnerObject Sender { get; }

        public OnSpikeCollisionEventArgs(RunnerObject sender)
        {
            Sender = sender;
        }
    }

    public event Action<OnSpikeCollisionEventArgs> OnSpikeCollision = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SpikeGroup>(out var spike))
        {
            if (spike.IsActive)
                OnSpikeCollision?.Invoke(new OnSpikeCollisionEventArgs(this));
        }
    }
}
