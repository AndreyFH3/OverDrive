using UnityEngine;
using Zenject;

public class PlayerPresenter
{

    private PlayerInputViewModel inputVM;

    [Inject]
    public void Init(PlayerInputViewModel input, PlayerView view)
    {
        inputVM = input;
    }
}
