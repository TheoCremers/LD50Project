using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _baseAttackCooldown = 1f;
    [SerializeField] private float _baseMoveSpeed = 5f;

    public UnityEvent<Vector2> RangedAttackEvent;
    public UnityEvent MeleeAttackEvent;

    private Vector2 _inputDirection = Vector2.zero;
    private Vector2 _targetDirection = Vector2.zero;
    private bool _shootButtonDown = false;

    private Vector2 _velocity = Vector2.zero;
    private float _attackCooldownRemaining = 0f;

    private float _moveSpeedModifier = 1f;
    private float _attackCooldownModifier = 1f;

    void Start ()
    {
    }

    void Update()
    {
        GetInput();

        ApplyMovement();

        ApplyActions();

        UpdateTimers();
    }

    private void OnDestroy ()
    {
        RangedAttackEvent.RemoveAllListeners();
        MeleeAttackEvent.RemoveAllListeners();
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_targetDirection);
    }

    private void GetInput ()
    {
        _inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputDirection != Vector2.zero)
        {
            _inputDirection.Normalize();
        }

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        _targetDirection = (mouseWorldPosition - transform.position).normalized;

        _shootButtonDown = Input.GetAxis("Fire1") > 0f;
    }

    private void ApplyMovement ()
    {
        _velocity = _inputDirection * _baseMoveSpeed * _moveSpeedModifier * Time.deltaTime;

        transform.Translate(_velocity);
    }

    private void ApplyActions()
    {
        if (_shootButtonDown)
        {
            if (_attackCooldownRemaining <= 0f)
            {
                RangedAttackEvent?.Invoke(_targetDirection);
                _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
            }
        }
    }

    private void UpdateTimers ()
    {
        if (_attackCooldownRemaining > 0f)
        {
            _attackCooldownRemaining -= Time.deltaTime;
            if (_attackCooldownRemaining < 0f)
            {
                _attackCooldownRemaining = 0f;
            }
        }
    }
}
