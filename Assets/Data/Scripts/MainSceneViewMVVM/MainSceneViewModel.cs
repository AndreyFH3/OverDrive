using UniRx;
using UnityEngine;

public class MainSceneViewModel
{
    private MainSceneModel _model;
    private MainSceneView _view;
    public MainSceneViewModel(MainSceneModel model, MainSceneView view)
    {
        _model = model;
        _view = view;

        Subscribe();
    }

    private void Subscribe()
    {
        _view.StartButton.OnClickAsObservable()
            .Subscribe(_ => 
            { 
                _model.LoadScene();
            });
    }
}