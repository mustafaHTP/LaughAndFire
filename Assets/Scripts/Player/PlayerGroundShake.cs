using Cinemachine;
using UnityEngine;

public class PlayerGroundShake : MonoBehaviour
{
    [SerializeField] private float _yVelocityToShake;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentVelocity;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void FixedUpdate()
    {
        _currentVelocity = _rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentVelocity.y < _yVelocityToShake)
        {
            _impulseSource.GenerateImpulse();
        }
    }
}
