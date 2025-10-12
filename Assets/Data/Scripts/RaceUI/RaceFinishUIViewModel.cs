using System;
using UniRx;
using UnityEngine.SceneManagement;

public class RaceFinishUIViewModel
{
    private RaceController _controller;
    private Subject<Unit> _onFinishStream = new();

    public IObservable<Unit> FinishStream => _onFinishStream;

    public RaceFinishUIViewModel(RaceController controller)
    {
        _controller = controller;
        _controller.OnRaceFinished += SetActive;
    }

    public void SetActive()
    {
        _onFinishStream.OnNext(Unit.Default);
    }

    public void LeaveRace()
    {
        SceneManager.LoadScene("MainScene");
    }

}