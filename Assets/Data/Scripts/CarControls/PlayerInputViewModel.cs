using UniRx;
using UnityEngine;

public class PlayerInputViewModel
{
    public readonly ReactiveProperty<float> RightTrigger = new ReactiveProperty<float>(0f);
    public readonly ReactiveProperty<float> LeftTrigger = new ReactiveProperty<float>(0f);
    public readonly ReactiveProperty<Vector2> Move = new ReactiveProperty<Vector2>(Vector2.zero);
    public readonly ReactiveProperty<bool> Brakes = new ReactiveProperty<bool>(false);

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
