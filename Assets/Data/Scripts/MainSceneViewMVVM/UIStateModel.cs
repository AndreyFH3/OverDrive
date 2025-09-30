using UnityEngine;

public class UIStateModel
{
    public MainSceneUI CurrentUISelected { get; private set; }
    public void SetCurrentStateController(MainSceneUI type)
    {
        CurrentUISelected = type;
    }
}

public enum MainSceneUI { Menu, CarSelect}