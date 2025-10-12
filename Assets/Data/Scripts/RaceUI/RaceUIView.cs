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
    [SerializeField] private TextMeshProUGUI _timerStartText;
    
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
        vm.TimerStart
            .Subscribe(SetStartTimer)
            .AddTo(this);
        vm.RaceState
            .Subscribe(DisableTimer)
            .AddTo(this);
    }

    private void SetSpeed(float value)
    {
        float speedRB = value * 3.6f;

        _speedText.text = $"{Mathf.Round(speedRB)}km/h";
    }

    private void SetLaps(int current, int target)
    {
        _lapsText.text = $"Laps: {current}/{target}";
    }

    private void SetTimer(float value)
    {
        _timerText.text = $"{(int)(value / 60)}:{(int)(value % 60)}";
    }

    private void DisableTimer(RaceState state)
    {
        if (!_timerStartText.IsActive() || RaceState.Prepare == state) return;
            _timerStartText.gameObject.SetActive(false);
    }

    private void SetStartTimer(float value)
    {
        if(value <= 0) 
            _timerStartText.gameObject.SetActive(false);
        _timerStartText.text = $"{Mathf.RoundToInt(value)}";
    }
}
