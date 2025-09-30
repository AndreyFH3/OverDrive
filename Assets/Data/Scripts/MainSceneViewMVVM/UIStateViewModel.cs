using System;
using UniRx;
using UnityEngine;

public class UIStateViewModel
{
    private UIStateModel _model;
    private readonly Subject<MainSceneUI> _mainSceneUI = new Subject<MainSceneUI>();

    public IObservable<MainSceneUI> MainSceneUIObservable => _mainSceneUI;

    public UIStateViewModel(UIStateModel model)
    {
        _model = model;
    }

    public void SetConditions(MainSceneUI type)
    {
        _model.SetCurrentStateController(type);
        _mainSceneUI.OnNext(_model.CurrentUISelected);
    }
}
