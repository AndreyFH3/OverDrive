using System;
using UniRx;
using UnityEngine;

public class CheckpointView : MonoBehaviour
{
    [SerializeField] private int id;
    public int Id => id;
    private readonly Subject<(int id, Collider other)> _onPlayerEntered = new();
    public IObservable<(int id, Collider other)> OnPlayerEntered => _onPlayerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerView pv))
            _onPlayerEntered.OnNext((id, other));
    }

    private void OnDestroy()
    {
        _onPlayerEntered.OnCompleted();
        _onPlayerEntered.Dispose();
    }
}
