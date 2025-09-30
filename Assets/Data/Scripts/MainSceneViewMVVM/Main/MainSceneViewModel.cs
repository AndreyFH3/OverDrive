using UniRx;
using UnityEngine;

public class MainSceneViewModel
{
    private MainSceneModel _model;
    private MainSceneView _view;
    public MainSceneViewModel(MainSceneModel model, MainSceneView view)
    {
        _model = model;
    }

    public void OnLoadScene()
    {
        _model.LoadScene();
    }
    
    public void Exit()
    {
        _model.Exit();
    }
}