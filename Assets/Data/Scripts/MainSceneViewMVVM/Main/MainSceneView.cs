using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainSceneView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    private MainSceneViewModel _vm;

    [Inject]
    public void Init(MainSceneViewModel vm, UIStateViewModel uiVM)
    {
        _vm = vm;
        uiVM.MainSceneUIObservable
            .Subscribe(SetActive)
            .AddTo(this);

        _startButton.OnClickAsObservable()
            .Subscribe(_ => uiVM.SetConditions(MainSceneUI.CarSelect))
            .AddTo(this);
        
        _exitButton.OnClickAsObservable()
            .Subscribe(_ => _vm.Exit())
            .AddTo(this);
    }


    private void SetActive(MainSceneUI type)
    {
        gameObject.SetActive(MainSceneUI.Menu == type);
    }
}
