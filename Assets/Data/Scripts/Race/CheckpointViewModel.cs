using System;
using UniRx;
using UnityEngine;
using Zenject;

public class CheckpointViewModel
{
    private readonly CheckpointModel _model;
    private readonly CheckpointView _view;
    private readonly CompositeDisposable _disposables = new();

    private readonly Subject<int> _onCheckpointPassed = new();
    public IObservable<int> OnCheckpointPassed => _onCheckpointPassed;

    [Inject]
    public CheckpointViewModel(CheckpointModel model, CheckpointView view)
    {
        _model = model;
        _view = view;
        Initialize();
    }

    private void HandlePlayerEntered(int id, Collider other)
    {
        if (id == _model.Id)
        {
            _model.MarkPassed();
            Debug.Log($"Checkpoint {id} passed by player");
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Initialize()
    { 
        _view.OnPlayerEntered
            .Subscribe(tuple => { HandlePlayerEntered(tuple.id, tuple.other); })
            .AddTo(_view);

        _model.OnPassed += id => _onCheckpointPassed.OnNext(id);
    }
    public class CheckpointViewModelFactory : PlaceholderFactory<CheckpointModel, CheckpointView, CheckpointViewModel> { }

}