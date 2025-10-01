using System;
using UniRx;
using UnityEngine;
using Zenject;

public class CarSetterViewModel
{
    private CarSetterModel _model;
    private CarContainer _carContainer;
    private DiContainer _diContainer;

    private readonly Subject<Transform> _carTransformStream = new Subject<Transform>();

    public IObservable<Transform> LoadCarTransformStream => _carTransformStream;

    public CarSetterViewModel(CarSetterModel model, CarContainer carContainer, DiContainer diContainer)
    {
        _model = model;
        _carContainer = carContainer;
        _diContainer = diContainer;
    }

    public void LoadCar()
    {
        _carContainer.LoadCar(_model.CarId, transform =>
        {
            var instance = GameObject.Instantiate(transform);
            _diContainer.InjectGameObject(instance.gameObject);
            _carTransformStream.OnNext(instance);
        });
    }
}