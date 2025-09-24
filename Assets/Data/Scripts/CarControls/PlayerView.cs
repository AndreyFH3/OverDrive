using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private WheelCollider _rightFrontCollider;
    [SerializeField] private WheelCollider _leftFrontCollider;
    [SerializeField] private WheelCollider _rightBackCollider;
    [SerializeField] private WheelCollider _leftBackCollider;

    [SerializeField] private Transform _rightFrontTransform;
    [SerializeField] private Transform _leftFrontTransform;
    [SerializeField] private Transform _rightBackTransform;
    [SerializeField] private Transform _leftBackTransform;

    #region modelData
    [SerializeField] private float _motorTorque = 2500;
    [SerializeField] private float _motorTorqueReverse = 500;
    [SerializeField] private float _steerAngle = 30;
    [SerializeField] private float _brakeForce = 800;
    #endregion

    private PlayerInputViewModel inputVM;
    [Inject]
    public void Init(PlayerInputViewModel vm)
    {
        inputVM = vm;


        inputVM.Move
            .ObserveOnMainThread()
            .Subscribe(OnMove)
            .AddTo(this);

        inputVM.RightTrigger
            .ObserveOnMainThread()
            .Subscribe(Move)
            .AddTo(this);

        inputVM.LeftTrigger
            .ObserveOnMainThread()
            .Subscribe(MoveReverse)
            .AddTo(this);

        inputVM.Brakes
            .ObserveOnMainThread()
            .Subscribe(Brake)
            .AddTo(this);
    }

    private void Brake(bool value)
    {
        var force = value ? _brakeForce : 0;
        //_rightFrontCollider.brakeTorque = force;
        //_leftFrontCollider.brakeTorque = force;
        _rightBackCollider.brakeTorque = force;
        _leftBackCollider.brakeTorque = force;
    }

    private void Move(float value)
    {
        _rightBackCollider.motorTorque = value * _motorTorque;
        _leftBackCollider.motorTorque = value * _motorTorque;
        //_rightFrontCollider.motorTorque = value * _motorTorque;
        //_leftFrontCollider.motorTorque = value * _motorTorque;
    }
    private void MoveReverse(float value)
    {
        _rightBackCollider.motorTorque = -value * _motorTorqueReverse;
        _leftBackCollider.motorTorque = -value * _motorTorqueReverse;
        //_rightFrontCollider.motorTorque = -value * _motorTorque;
        //_leftFrontCollider.motorTorque = -value * _motorTorque;
    }

    private void OnMove(Vector2 move)
    {
        _rightFrontCollider.steerAngle = move.x * _steerAngle;
        _leftFrontCollider.steerAngle = move.x * _steerAngle;
    }
    private void FixedUpdate()
    {
        UpdateWheelPose(_rightFrontCollider, _rightFrontTransform);
        UpdateWheelPose(_leftFrontCollider, _leftFrontTransform);
        UpdateWheelPose(_rightBackCollider, _rightBackTransform);
        UpdateWheelPose(_leftBackCollider, _leftBackTransform);
    }
    private void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);

        trans.position = pos;
        trans.rotation = rot;
    }
}
