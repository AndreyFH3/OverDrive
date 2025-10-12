using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RaceFinishUIView : MonoBehaviour
{
    [SerializeField] private Button _leaveButton;

    [Inject]
    public void Init(RaceFinishUIViewModel vm)
    {
        gameObject.SetActive(false);
        vm.FinishStream
            .Subscribe(_ => gameObject.SetActive(true))
            .AddTo(this);

        _leaveButton
            .OnClickAsObservable()
            .Subscribe(_ => vm.LeaveRace())
            .AddTo(this);
    }
}
