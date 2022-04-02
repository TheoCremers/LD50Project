using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;
    [SerializeField] private float _baseAttackCooldown = 1f;
    [SerializeField] private float _baseMoveSpeed = 5f;

    //public UnityEvent<Vector2> RangedAttackEvent;
    //public UnityEvent MeleeAttackEvent;

    private Rigidbody2D _rigidBody = null;
    private RangedAttack _rangedAttack = null;

    private Vector2 _inputDirection = Vector2.zero;
    private Vector2 _targetDirection = Vector2.zero;
    private bool _shootButtonDown = false;

    private Vector2 _velocity = Vector2.zero;
    private float _attackCooldownRemaining = 0f;

    private float _moveSpeedModifier = 1f;
    private float _attackCooldownModifier = 1f;

    void Start ()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rangedAttack = GetComponent<RangedAttack>();
    }

    void Update()
    {
        GetInput();

        ApplyMovement();

        ApplyActions();

        UpdateTimers();
    }

    void Awake()
    {
        Instance = this;
    }

    private void OnDestroy ()
    {
        //RangedAttackEvent.RemoveAllListeners();
        //MeleeAttackEvent.RemoveAllListeners();
        Instance = null;
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
        _velocity = _inputDirection * _baseMoveSpeed * _moveSpeedModifier;

        _rigidBody.velocity = _velocity;
    }

    private void ApplyActions()
    {
        if (_shootButtonDown)
        {
            if (_attackCooldownRemaining <= 0f)
            {
                //RangedAttackEvent?.Invoke(_targetDirection);
                _rangedAttack.Fire(_targetDirection);
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
