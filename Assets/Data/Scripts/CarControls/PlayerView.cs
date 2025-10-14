using System;
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

    [SerializeField]
    private Rigidbody _rb;
    #region modelData
    [SerializeField] private float _motorTorque = 2500;
    [SerializeField] private float _motorTorqueReverse = 500;
    [SerializeField] private float _steerAngle = 30;
    [SerializeField] private float _brakeForce = 800;
    #endregion
    
    private Vector3 _lastVelocity;
    private float flwTorque = 0;
    private float frwTorque = 0;
    private float blwTorque = 0;
    private float brwTorque = 0;
    private bool isPaused;
    private bool _isForward = false;
    private bool _isBack = false;
    private float _forwardValue = 0;
    private float _reverseValue = 0;

    private PlayerInputViewModel inputVM;

    private Subject<Unit> _magnitudeRbStream = new();
    public Rigidbody Rigidbody => _rb;
    public IObservable<Unit> MagnitudeRBStream => _magnitudeRbStream;

    [Inject]
    public void Init(PlayerInputViewModel vm, RaceUIViewModel uiVM)
    {
        inputVM = vm;
        uiVM.RaceState
            .Subscribe(state => 
            {
                if (state != RaceState.Running && state != RaceState.Finished)
                {
                    _lastVelocity = _rb.linearVelocity;
                    _rb.linearVelocity = Vector3.zero;
                    _rb.freezeRotation = true;

                    flwTorque = _rightBackCollider.motorTorque;
                    frwTorque = _leftBackCollider.motorTorque;
                    blwTorque = _rightFrontCollider.motorTorque;
                    brwTorque = _leftFrontCollider.motorTorque;

                    _rightBackCollider.motorTorque = 0;
                    _leftBackCollider.motorTorque = 0;
                    _rightFrontCollider.motorTorque = 0;
                    _leftFrontCollider.motorTorque = 0;

                    isPaused = true;
                }
                else
                { 
                    isPaused = false;
                    _rb.linearVelocity = _lastVelocity;
                    _rb.freezeRotation = false;
                    _rightBackCollider.motorTorque = flwTorque;
                    _leftBackCollider.motorTorque = frwTorque;
                    _rightFrontCollider.motorTorque = blwTorque;
                    _leftFrontCollider.motorTorque = brwTorque;
                }
            })
            .AddTo(this);

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
        if (isPaused) return;
        var force = value ? _brakeForce : 0;
        _rightFrontCollider.brakeTorque = force * .7f;
        _leftFrontCollider.brakeTorque = force * .7f;
        _rightBackCollider.brakeTorque = force * .3f;
        _leftBackCollider.brakeTorque = force * .3f;
    }

    private void Move(float value)
    {
        _isForward = value > 0.1;
        _forwardValue = value;
    }
    private void MoveReverse(float value)
    {
        _isBack = value > 0;
        _reverseValue = -value;
    }

    private void OnMove(Vector2 move)
    {
        _rightFrontCollider.steerAngle = move.x * _steerAngle;
        _leftFrontCollider.steerAngle = move.x * _steerAngle;
    }
    private void FixedUpdate()
    {
        if(isPaused) 
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }
        if (_isBack)
        {
            _rightBackCollider.motorTorque = _reverseValue * _motorTorqueReverse;
            _leftBackCollider.motorTorque = _reverseValue * _motorTorqueReverse;
        }
        else if (_isForward)
        {
            _leftBackCollider.motorTorque = _forwardValue * _motorTorque;
            _rightBackCollider.motorTorque = _forwardValue * _motorTorque;
        }
        else
        { 
            _leftBackCollider.motorTorque = 0;
            _rightBackCollider.motorTorque = 0;
        } 
        UpdateWheelPose(_rightFrontCollider, _rightFrontTransform);
        UpdateWheelPose(_leftFrontCollider, _leftFrontTransform);
        UpdateWheelPose(_rightBackCollider, _rightBackTransform);
        UpdateWheelPose(_leftBackCollider, _leftBackTransform);
        _magnitudeRbStream.OnNext(Unit.Default);
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
