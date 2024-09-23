using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ControllerBase
{
    [Header("Behaviour")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Transform _forwardCollisionDetector;
    [SerializeField] private Transform _downwardCollisionDetector;

    private Rigidbody2D _rb;
    private int _direction = 1;
    private bool _isMovementEnabled;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _isMovementEnabled = true;
    }

    private void FixedUpdate()
    {
        if (_isMovementEnabled)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (_direction == 1)
        {
            Unit.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            Unit.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    ///<summary>
    ///1 para olhar para direita
    ///<br/>-1 para olhar para esquerda
    ///</summary>
    public void DisableMovement(int direction = 0)
    {
        _isMovementEnabled = false;
        _rb.velocity = Vector3.zero;
        if (direction != 0)
        {
            _direction = direction;
        }
    }

    private void Move()
    {
        RaycastHit2D hitForward = Physics2D.Raycast(_forwardCollisionDetector.position, transform.right, 1f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(_forwardCollisionDetector.position, transform.right, Color.red);
        if (hitForward)
        {
            // Bateu na parede
            _direction *= -1;
        }

        RaycastHit2D hitDownward = Physics2D.Raycast(_downwardCollisionDetector.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(_downwardCollisionDetector.position, Vector2.down, Color.red);
        if (!hitDownward)
        {
            // Prestes a sair da plataforma
            _direction *= -1;
        }

        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }
}