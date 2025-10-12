using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RacePauseUIView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitMenuButton;
    
    [Inject]
    public void Init(RacePauseUIViewModel vm)
    {
        gameObject.SetActive(false);
        vm.PauseCheckStream
            .Subscribe(gameObject.SetActive)
            .AddTo(this);

        _continueButton
            .OnClickAsObservable()
            .Subscribe(_ => vm.ContinueRace())
            .AddTo(this);

        _exitMenuButton
            .OnClickAsObservable()
            .Subscribe(_ => vm.LoadBaseScene())
            .AddTo(this);
    }
}
