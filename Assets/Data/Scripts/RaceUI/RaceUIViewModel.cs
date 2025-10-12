using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public enum RaceState { Prepare, Countdown, Running, Finished, Paused }
public class RaceUIViewModel
{
    private PlayerView _playerView;
    private CancellationTokenSource _cts;
    private RaceController _controller;
    private Subject<(int, int)> _lapsStream = new();
    private readonly ReactiveProperty<float> _speed = new(0);
    private readonly ReactiveProperty<float> _timer = new(0);
    private readonly ReactiveProperty<float> _timerStart = new(3);
    private readonly ReactiveProperty<RaceState> _raceState = new(global::RaceState.Prepare);
    public IReadOnlyReactiveProperty<RaceState> RaceState => _raceState;
    public IObservable<(int, int)> LapsStreamObserver => _lapsStream;
    public IReadOnlyReactiveProperty<float> Speed => _speed;
    public IReadOnlyReactiveProperty<float> Timer => _timer;
    public IReadOnlyReactiveProperty<float> TimerStart => _timerStart;

    [Inject]
    public void Init(RaceController controller)
    {
        _controller = controller;
        _cts = new CancellationTokenSource();
        Subscribe();
    }

    public void BindPlayer(PlayerView playerView)
    {
        _playerView = playerView;
        _playerView.MagnitudeRBStream
            .Select(_ => playerView.Rigidbody.linearVelocity.magnitude)
            .Subscribe(currentSpeed =>
            {
                _speed.Value = currentSpeed;
            });
        StartRace();
    }

    private async UniTask StartRaceAsync(CancellationToken ct)
    {
        float timer = 3;
        try
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                _timerStart.Value = timer;
                await UniTask.WaitForEndOfFrame(ct);
            }
        }
        catch { }
        finally
        {
            _controller.StartRace();
            SetLap(1);
            if (!ct.IsCancellationRequested)
                Update(ct).Forget();
        }
    }

    private async UniTask Update(CancellationToken ct)
    {
        try
        {
            while (_raceState.Value != global::RaceState.Finished)
            {
                if(_raceState.Value == global::RaceState.Running)
                    _controller.Update(Time.deltaTime);
                await UniTask.WaitForEndOfFrame(ct);
            }
        }
        catch { }
    }

    private void Subscribe()
    {
        _controller.OnRaceStateChanged += SetRaceCondition;
        _controller.OnLapCompleted += SetLap;
        _controller.OnRaceFinished += RaceFinished;
        _controller.OnRaceTimerUpdated += SetTimer;
    }

    private void SetTimer(float timerValue)
    {
        _timer.Value = timerValue;
    }

    private void SetRaceCondition(RaceState state)
    {
        _raceState.Value = state;
    }

    private void RaceFinished()
    {
        SceneManager.LoadScene(0);
    }

    private void SetLap(int value)
    {
        var cartage = (value, _controller.LapsToComplete);
        _lapsStream.OnNext(cartage);
    }
    private void StartRace()
    {
        StartRaceAsync(_cts.Token).Forget();
    }
}
