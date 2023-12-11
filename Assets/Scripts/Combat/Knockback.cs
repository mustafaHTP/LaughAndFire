using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public event Action OnKnockbackStart;
    public event Action OnKnockbackEnd;

    [SerializeField] private float _knockbackTime = 0.2f;

    private Vector2 _hitDirection;
    private float _knockbackPower;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockBackForce;
        OnKnockbackEnd += StopKnockRoutine;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockBackForce;
        OnKnockbackEnd -= StopKnockRoutine;
    }

    public void ApplyKnockback(Vector3 hitDirection, float knockbackPower)
    {
        _hitDirection = hitDirection;
        _knockbackPower = knockbackPower;

        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockBackForce()
    {
        _rigidbody.AddForce(_hitDirection.normalized * _knockbackPower);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    private void StopKnockRoutine()
    {
        _rigidbody.velocity = Vector2.zero;
    }

}
