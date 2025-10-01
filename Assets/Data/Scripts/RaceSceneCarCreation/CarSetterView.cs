using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CarSetterView : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private Transform spawnPosition;

    [Inject]
    public void Init(CarSetterViewModel vm)
    {
        vm.LoadCarTransformStream
            .Subscribe(SetCar)
            .AddTo(this);
        vm.LoadCar();
    }

    public void SetCar(Transform car)
    {
        car.position = spawnPosition.position;
        car.rotation = spawnPosition.rotation;
        _camera.Target.TrackingTarget = car;
    }
}
