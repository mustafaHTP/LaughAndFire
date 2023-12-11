using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnPlayerHit;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    [Header("Damage")]
    [SerializeField] private float _knockbackPower = 1000f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _damageAmount = 1;

    private int _currentDirection;
    private bool _hasAttacked = false;
    private float _attackTimer = 0f;
    private Rigidbody2D _rigidBody;
    private ColorChanger _colorChanger;

    public void Init(Color color)
    {
        _colorChanger.SetDefaultColor(color);
    }

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    private void Update()
    {
        if (_hasAttacked)
        {
            _attackTimer += Time.deltaTime;
        }

        if(_attackTimer >= _attackCooldown)
        {
            _hasAttacked = false;
            _attackTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_hasAttacked)
        {
            _hasAttacked = true;
            OnPlayerHit?.Invoke(this);
            IHitable hitable = collision.gameObject.GetComponent<IHitable>();
            hitable?.TakeHit();

            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(transform.position, _damageAmount, _knockbackPower);
        }
    }

    private void Move()
    {
        Vector2 newVelocity = new(_currentDirection * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = newVelocity;
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }
}
