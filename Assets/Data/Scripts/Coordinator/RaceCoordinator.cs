using System;
using UniRx;
using Zenject;

public class RaceCoordinator : IDisposable
{
    private readonly CarSetterViewModel _carSetterVM;
    private readonly RaceUIViewModel _raceUIVM;
    private readonly CompositeDisposable _disposables = new();

    public RaceCoordinator(CarSetterViewModel carSetterVM, RaceUIViewModel raceUIVM)
    {
        _carSetterVM = carSetterVM;
        _raceUIVM = raceUIVM;
        _carSetterVM.LoadCarTransformStream
            .Select(t => t.GetComponent<PlayerView>())
            .Subscribe(playerView => _raceUIVM.BindPlayer(playerView))
            .AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}
