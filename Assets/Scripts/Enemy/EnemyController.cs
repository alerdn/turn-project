using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Unit EnemyUnit => _enemyUnit;

    [Header("Behaviour")]
    [SerializeField] private Unit _enemyUnit;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Transform _forwardCollisionDetector;
    [SerializeField] private Transform _downwardCollisionDetector;

    private Rigidbody2D _rb;
    private int direction = 1;
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

    public void DisableMovement()
    {
        _isMovementEnabled = false;
        _rb.velocity = Vector3.zero;
    }

    private void Move()
    {
        RaycastHit2D hitForward = Physics2D.Raycast(_forwardCollisionDetector.position, transform.right, 1f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(_forwardCollisionDetector.position, transform.right, Color.red);
        if (hitForward)
        {
            // Bateu na parede
            direction *= -1;
        }

        RaycastHit2D hitDownward = Physics2D.Raycast(_downwardCollisionDetector.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(_downwardCollisionDetector.position, Vector2.down, Color.red);
        if (!hitDownward)
        {
            // Prestes a sair da plataforma
            direction *= -1;
        }

        if (direction == 1)
        {
            _enemyUnit.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            _enemyUnit.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        _rb.velocity = new Vector2(direction * _moveSpeed, _rb.velocity.y);
    }
}