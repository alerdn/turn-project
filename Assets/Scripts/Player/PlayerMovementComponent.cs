using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _jumpBufferTime;
    public bool CanCoyoteJump => _canCoyoteJump && _time < _timeLeftGround + _coyoteJumpTime;

    [Header("Layers")]
    [Tooltip("Set this to the layer your player is on")]
    [SerializeField] private LayerMask _playerLayer;

    [Space]
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _acceleration = 120f;
    [SerializeField] private float _deceleration = 90f;
    [SerializeField] private float _jumpForce = 30f;
    [SerializeField] private float _coyoteJumpTime = .15f;
    [SerializeField] private float _jumpBufferTime = .15f;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    private float _time;
    private float _timeLeftGround;

    private bool _cachedQueryStartInColliders;
    private bool _grounded;
    private bool _canCoyoteJump;
    private float _gravityScale;
    private bool _bufferedJumpUsable;
    private bool _hasJumpToConsume;
    private float _timeJumpWasPressed;
    private InputReader _input;
    private Transform _graph;

    public void Init(InputReader input, Transform graph)
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        _gravityScale = _rb.gravityScale;

        _input = input;
        _input.JumpEvent += OnJump;

        _graph = graph;
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

        if (_input.MovementAxis > 0)
        {
            _graph.eulerAngles = Vector3.zero;
        }
        else if (_input.MovementAxis < 0)
        {
            _graph.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        _rb.velocity = new Vector2(horizontalVelocity, _rb.velocity.y);
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

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void OnJump()
    {
        _hasJumpToConsume = true;
        _timeJumpWasPressed = _time;
    }
}