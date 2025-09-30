using System;
using UniRx;
using UnityEngine;

public class CarSelectViewModel
{
    private CarSelectModel _model;
    private CarContainer _container;
    private SelectedCarHelper _helper;

    private readonly Subject<Transform> _carTransformStream = new Subject<Transform>();

    public IObservable<Transform> SelectedCarTransform => _carTransformStream;
    public CarSelectViewModel(CarSelectModel model, CarContainer container, SelectedCarHelper helper)
    {
        _helper = helper;
        _model = model;
        _container = container;
        _container.LoadCar(_model.CurrentCarId, transform =>
        {
            _carTransformStream.OnNext(transform);
        });
    }

    public void SetNextCar()
    {
        _model.NextCarSelect();
        _container.LoadCar(_model.CurrentCarId, transform =>
        {
            _carTransformStream.OnNext(transform);
        });
    }
    public void SetPreviousCar()
    {
        _model.PreviousCarSelect();
        _container.LoadCar(_model.CurrentCarId, transform =>
        {
            _carTransformStream.OnNext(transform);

        });
    }

    public void StartGame()
    {
        _helper.SetId(_model.CurrentCarId);
        _model.LoadScene();
    }
}
