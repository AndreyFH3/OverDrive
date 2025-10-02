using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;
public class RaceUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _lapsText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Inject]
    public void Init(RaceUIViewModel vm)
    {
        vm.LapsStreamObserver
            .Subscribe(data => SetLaps(data.Item1, data.Item2))
            .AddTo(this);
        vm.Speed
            .Subscribe(SetSpeed)
            .AddTo(this);
        vm.Timer
            .Subscribe(SetTimer)
            .AddTo(this);
    }

    public void SetSpeed(float value)
    {
        _speedText.text = $"{Mathf.Round(value)}km/h";
    }

    public void SetLaps(int current, int target)
    {
        _lapsText.text = $"Laps: {current}/{target}";
    }

    public void SetTimer(float value)
    {
        _timerText.text = $"{value / 60}:{value % 60}";
    }
}
