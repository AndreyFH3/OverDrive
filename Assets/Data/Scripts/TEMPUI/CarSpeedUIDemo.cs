using TMPro;
using UnityEngine;

public class CarSpeedUIDemo : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private WheelCollider _wheelCollider;

    [SerializeField] private TextMeshProUGUI _speedRB;
    [SerializeField] private TextMeshProUGUI _speedWC;


    private void Update()
    {
        // �������� �� Rigidbody (����������, � ��/�)
        float speedRB = _rigidbody.linearVelocity.magnitude * 3.6f;

        // �������� �� WheelCollider (������������ ����� RPM)
        float angularVelocity = _wheelCollider.rpm * 2f * Mathf.PI / 60f; // ���/�
        float linearVelocity = angularVelocity * _wheelCollider.radius;   // �/�
        float speedWC = linearVelocity * 3.6f;                            // ��/�

        _speedRB.text = $"RB: {Mathf.RoundToInt(speedRB)} km/h";
        _speedWC.text = $"WC: {Mathf.RoundToInt(speedWC)} km/h";
    }
}
