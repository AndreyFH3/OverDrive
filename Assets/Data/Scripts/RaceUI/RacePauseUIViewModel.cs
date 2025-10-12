using System;
using UniRx;
using UnityEngine.SceneManagement;

public class RacePauseUIViewModel
{
    private RaceController _controller;
    private Subject<bool> _onPause = new();

    public IObservable<bool> PauseCheckStream => _onPause;

    public RacePauseUIViewModel(RaceController controller)
    {
        _controller = controller;
        _controller.OnRaceStateChanged += SetActiveRaceUI;
    }

    public void ContinueRace()
    {
        _controller.ResumeRace();
    }

    private void SetActiveRaceUI(RaceState state)
    {
        _onPause.OnNext(state == RaceState.Paused);
    }

    public void LoadBaseScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}