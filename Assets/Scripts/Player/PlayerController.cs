using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool CanCoyoteJump => _canCoyoteJump && _time < _timeLeftGround + _coyoteJumpTime;

    [Header("Layers")]
    [Tooltip("Set this to the layer your player is on")]
    [SerializeField] private LayerMask _playerLayer;

    [Header("Input")]
    [SerializeField] private InputReader _input;

    [Header("Player")]
    [SerializeField] private Unit _playerUnit;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _acceleration = 100f;
    [SerializeField] private float _deceleration = 60f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _coyoteJumpTime = .15f;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    private float _time;
    private float _timeLeftGround;

    private bool _cachedQueryStartInColliders;
    private bool _grounded;
    private bool _canCoyoteJump;
    private float _gravityScale;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Start()
    {
        _input.JumpEvent += OnJump;

        _gravityScale = _rb.gravityScale;
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        Move();
    }

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, 0.05f, ~_playerLayer);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _canCoyoteJump = true;
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _timeLeftGround = _time;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private void Move()
    {
        _rb.gravityScale = _gravityScale;
        if (!_grounded && _rb.velocity.y <= 0)
        {
            _rb.gravityScale = _gravityScale * 1.5f;
        }

        float horizontalVelocity;
        if (_input.MovementAxis == 0)
        {
            horizontalVelocity = Mathf.MoveTowards(_rb.velocity.x, 0f, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity = Mathf.MoveTowards(_rb.velocity.x, _input.MovementAxis * _moveSpeed, _acceleration * Time.fixedDeltaTime);
        }

        Debug.Log($"horizontalVelocity: {horizontalVelocity}");
        _rb.velocity = new Vector2(horizontalVelocity, _rb.velocity.y);
    }

    private void OnJump()
    {
        if (!_grounded && !CanCoyoteJump) return;
        _canCoyoteJump = false;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
