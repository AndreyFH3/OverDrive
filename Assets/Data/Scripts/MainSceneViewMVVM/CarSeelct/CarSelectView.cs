using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CarSelectView : MonoBehaviour
{
    [SerializeField] private Button _nextCar;
    [SerializeField] private Button _previousCar;
    [SerializeField] private Button _closeSelect;
    [SerializeField] private Button _startSelect;

    [SerializeField] private Transform _parent;
    private Transform _currentTransform;

    [Inject]
    public void Init(CarSelectViewModel carVM, UIStateViewModel uiVM)
    {
        _nextCar.OnClickAsObservable()
            .Subscribe(_ => carVM.SetNextCar())
            .AddTo(this);
        
        _previousCar.OnClickAsObservable()
            .Subscribe(_ => carVM.SetPreviousCar())
            .AddTo(this);

        _closeSelect.OnClickAsObservable()
            .Subscribe(_ => uiVM.SetConditions(MainSceneUI.Menu))
            .AddTo(this);

        _startSelect.OnClickAsObservable()
            .Subscribe(_ => carVM.StartGame())
            .AddTo(this);

        uiVM.MainSceneUIObservable
            .Subscribe(SetActive)
            .AddTo(this);
        
        carVM.SelectedCarTransform
            .Subscribe(SetCar)
            .AddTo(this);
    }

    public void SetCar(Transform newTransform)
    {
        if (_currentTransform != null)
            Destroy(_currentTransform.gameObject);

        _currentTransform = Instantiate(newTransform, _parent);

        if (_currentTransform.TryGetComponent(out Rigidbody rb))
            GameObject.Destroy(rb);
        if (_currentTransform.TryGetComponent(out PlayerView pv))
            GameObject.Destroy(pv);
    }

    private void SetActive(MainSceneUI type)
    {
        gameObject.SetActive(MainSceneUI.CarSelect == type);
    }
}
