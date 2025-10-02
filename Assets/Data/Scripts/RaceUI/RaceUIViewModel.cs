using System;
using UniRx;

public class RaceUIViewModel
{
    private Subject<(int,int)> _lapsStream = new();
    private readonly ReactiveProperty<float> _speed = new(0);
    private readonly ReactiveProperty<float> _timer = new(0);
    
    public IObservable<(int,int)> LapsStreamObserver => _lapsStream;
    public IReadOnlyReactiveProperty<float> Speed => _speed;
    public IReadOnlyReactiveProperty<float> Timer => _timer;
    
    public void BindPlayer(PlayerView playerView)
    {
        playerView.MagnitudeRBStream
            .Select(_ => playerView.Rigidbody.linearVelocity.magnitude)
            .Subscribe(currentSpeed =>
            {
                _speed.Value = currentSpeed;
            });
        UnityEngine.Debug.Log("Car UI VM Init!");
    }
}
