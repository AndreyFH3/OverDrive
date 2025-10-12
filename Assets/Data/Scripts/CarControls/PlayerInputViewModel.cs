using UniRx;
using UnityEngine;

public class PlayerInputViewModel
{
    public readonly ReactiveProperty<float> RightTrigger = new ReactiveProperty<float>(0f);
    public readonly ReactiveProperty<float> LeftTrigger = new ReactiveProperty<float>(0f);
    public readonly ReactiveProperty<Vector2> Move = new ReactiveProperty<Vector2>(Vector2.zero);
    public readonly ReactiveProperty<bool> Brakes = new ReactiveProperty<bool>(false);
    public readonly ReactiveProperty<bool> Paused = new ReactiveProperty<bool>(false);
    private RaceController _controller;
    public PlayerInputViewModel(RaceController controller)
    {
        _controller = controller;
    }

    public void SetPaused(bool value)
    {
        if (_controller.RaceState == RaceState.Prepare) return;

        Paused.Value = value;
        if (value)
            _controller.PauseRace();
        else
            _controller.ResumeRace();
    }

    public  void SetBrakes(bool value)
    {
        Brakes.Value = value;
    }

    public void SetRightTrigger(float value) 
    { 
        RightTrigger.Value = value;
    }

    public void SetLeftTrigger(float value)
    { 
        LeftTrigger.Value = value;
    }
    public void SetMove(Vector2 value) => Move.Value = value;
}
