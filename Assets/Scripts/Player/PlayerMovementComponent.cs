using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public bool IsGrounded => _grounded;
    public Vector2 FrameVelocity => _frameVelocity;

    public bool CanCoyoteJump => _canCoyoteJump && _time < _timeLeftGround + _coyoteJumpTime;
    public bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _jumpBufferTime;

    [Header("Layers")]
    [Tooltip("Set this to the layer your player is on")]
    [SerializeField] private LayerMask _playerLayer;

    [Space]
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _airMoveSpeed = 10f;
    [SerializeField] private float _acceleration = 120f;
    [SerializeField] private float _groundDeceleration = 90f;
    [SerializeField] private float _airDeceleration = 40f;
    [SerializeField] private float _groundingForce = 0f;
    [SerializeField] private float _fallAcceleration = 90f;
    [SerializeField] private float _maxFallSpeed = 40f;
    [SerializeField] private float _jumpForce = 30f;
    [SerializeField] private float _coyoteJumpTime = .15f;
    [SerializeField] private float _jumpBufferTime = .15f;
    [SerializeField] private float _fallSpeedDampingChangeThreshold;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    private float _time;
    private float _timeLeftGround;

    private bool _cachedQueryStartInColliders;
    private bool _grounded;
    private bool _canCoyoteJump;
    private bool _bufferedJumpUsable;
    private bool _hasJumpToConsume;
    private float _timeJumpWasPressed;
    private InputReader _input;
    private Vector2 _frameVelocity;

    public void Init(InputReader input)
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _fallSpeedDampingChangeThreshold = CameraManager.Instance.FallSpeedDampingChangeThreshold;

        _input = input;
        _input.JumpEvent += OnJump;
    }

    private void OnDestroy()
    {
        _input.JumpEvent -= OnJump;
    }

    private void Update()
    {
        _time = Time.time;
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleMovement();
        HandleJump();
        HandleGravity();

        ApplyMovement();
    }

    public void ResetVelocity()
    {
        _frameVelocity = Vector2.zero;
    }

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
        bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, 0.05f, LayerMask.GetMask("Ground"));

        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _canCoyoteJump = true;
            _bufferedJumpUsable = true;
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _timeLeftGround = _time;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private void HandleMovement()
    {
        if (_input.MovementAxis == 0)
        {
            float deceleration = _grounded ? _groundDeceleration : _airDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            float moveSpeed = _grounded ? _moveSpeed : _airMoveSpeed;
            _frameVelocity.x = Mathf.MoveTowards(_rb.velocity.x, _input.MovementAxis * moveSpeed, _acceleration * Time.fixedDeltaTime);
        }

        if (_input.MovementAxis > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_input.MovementAxis < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void HandleJump()
    {
        if (!_hasJumpToConsume) return;

        if (!HasBufferedJump) return;

        if (_grounded || CanCoyoteJump) ExecuteJump();

        _hasJumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _timeJumpWasPressed = 0;
        _canCoyoteJump = false;
        _bufferedJumpUsable = false;

        _frameVelocity.y = _jumpForce;
    }

    private void OnJump()
    {
        _hasJumpToConsume = true;
        _timeJumpWasPressed = _time;
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _groundingForce;
        }
        else
        {
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_maxFallSpeed, _fallAcceleration * Time.fixedDeltaTime);
        }

        // If we are falling past a certain speed threshold
        if (_frameVelocity.y < _fallSpeedDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }

        // If we are stading still or moving up
        if (_frameVelocity.y >= 0f && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            // Reset so it can called again
            CameraManager.Instance.LerpedFromPlayerFalling = false;

            CameraManager.Instance.LerpYDamping(false);
        }
    }

    private void ApplyMovement()
    {
        _rb.velocity = _frameVelocity;
    }
}