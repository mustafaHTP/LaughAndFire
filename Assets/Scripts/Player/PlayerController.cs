using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public static event Action OnJump;
    public static event Action OnJetpack;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private BoxCollider2D _groundCheckCollider;

    [Header("General Config")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravityScale = 7f;
    [SerializeField] private float _gravityDelay = 1f;
    [SerializeField] private float _coyoteTime = 1f;
    [SerializeField] private float _maxVerticalFallVelocity = -20f;

    [Header("Jetpack Config")]
    [SerializeField] private float _jetpackTime;
    [SerializeField] private float _jetpackStrength;
    [SerializeField] private TrailRenderer _jetpackTR;
    private Coroutine _jetpackCoroutine;


    private float _airTimeCounter = 0f;
    private float _coyoteTimeCounter = 0f;

    private float _defaultGravityScale;
    private Vector2 _movementAmount;
    private Rigidbody2D _rigidBody;
    private bool _hasDoubleJumped = false;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    public Vector2 MoveInput => _frameInput.Move;
    public bool ThrowGrenadeInput => _frameInput.ThrowGrenade;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        _defaultGravityScale = _rigidBody.gravityScale;
    }

    private void Update()
    {
        GetInput();
        HandleSpriteFlip();

        Move();
        Jump();
        Jetpack();
        CountdownCoyoteTime();
        ApplyExtraGravity();

        if (IsGrounded())
            _hasDoubleJumped = false;
    }

    private void OnEnable()
    {
        OnJetpack += StartJetpack;
    }

    private void OnDisable()
    {
        OnJetpack -= StartJetpack;
    }

    public bool IsGrounded()
    {
        if (_groundCheckCollider.IsTouchingLayers(_groundLayer))
        {
            return true;
        }

        return false;
    }

    private void ApplyExtraGravity()
    {
        if (!IsGrounded())
        {
            _airTimeCounter += Time.deltaTime;
        }
        else
        {
            _airTimeCounter = 0f;
        }

        if (_airTimeCounter >= _gravityDelay)
        {
            _rigidBody.gravityScale = _extraGravityScale;
        }
        else
        {
            _rigidBody.gravityScale = _defaultGravityScale;
        }

        float verticalVelocity = _rigidBody.velocity.y;
        float clampedVelocity = Mathf.Max(verticalVelocity, _maxVerticalFallVelocity);
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, clampedVelocity);
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GetInput()
    {
        _frameInput = _playerInput.FrameInput;
        _movementAmount = new Vector2(_frameInput.Move.x * _moveSpeed, _rigidBody.velocity.y);
    }

    private void Move()
    {
        _rigidBody.velocity = new Vector2(_movementAmount.x, _rigidBody.velocity.y);
    }

    private void Jump()
    {
        if (!_frameInput.Jump) return;

        //First Jump
        if (_coyoteTimeCounter > 0f)
        {
            ApplyJumpForce();
            OnJump?.Invoke();
        }
        //Double Jump
        else if (!IsGrounded() && !_hasDoubleJumped)
        {
            ApplyJumpForce();
            OnJump?.Invoke();
            _airTimeCounter = 0f;
            _hasDoubleJumped = true;
        }
    }

    private void CountdownCoyoteTime()
    {
        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpStrength);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void Jetpack()
    {
        if (!_frameInput.Jetpack || _jetpackCoroutine != null) return;

        OnJetpack?.Invoke();
    }

    private void StartJetpack()
    {
        _jetpackCoroutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine()
    {
        float jetpackTime = 0f;
        while (jetpackTime < _jetpackTime)
        {
            jetpackTime += Time.deltaTime;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jetpackStrength);
            _jetpackTR.emitting = true;
            yield return null;
        }

        _jetpackTR.emitting = false;
        _jetpackCoroutine = null;
    }
}