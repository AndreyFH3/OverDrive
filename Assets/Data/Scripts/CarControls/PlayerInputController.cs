using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputController : MonoBehaviour
{
    private RaceController _controls;
    private PlayerInputViewModel viewModel;

    [Inject]
    public void Init(PlayerInputViewModel vm, RaceController controls)
    {
        viewModel = vm;
        _controls = controls;
        _controls.CarController.Break.canceled += UnPressed;
        _controls.CarController.Break.performed += Pressed;
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void Pressed(InputAction.CallbackContext cntx) { viewModel.SetBrakes(true); }
    private void UnPressed(InputAction.CallbackContext cntx) { viewModel.SetBrakes(false); }

    private void Update()
    {
        viewModel.SetRightTrigger(_controls.CarController.Boost.ReadValue<float>());
        viewModel.SetLeftTrigger(_controls.CarController.Reverse.ReadValue<float>());
        viewModel.SetMove(_controls.CarController.Turning.ReadValue<Vector2>());
    }
}
