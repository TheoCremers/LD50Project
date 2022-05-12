using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    [SerializeField] private float _baseAttackCooldown = 1f;
    [SerializeField] private float _baseMoveSpeed = 5f;
    [SerializeField] private float _baseLv1SummonSpeed = 4f;
    [SerializeField] private float _baseLv2SummonSpeed = 5f;
    [SerializeField] private float _baseLv3SummonSpeed = 6f;

    //public UnityEvent<Vector2> RangedAttackEvent;
    //public UnityEvent MeleeAttackEvent;

    private Rigidbody2D _rigidBody = null;

    [HideInInspector] public RangedAttack RangedAttack = null;
    [HideInInspector] public MeleeAttack MeleeAttack = null;
    [HideInInspector] public Summoner Summoner = null;
    [HideInInspector] public PlayerLeveling LevelingSystem = null;
    public Damagable HitpointData = null;

    public SpriteRenderer SpriteRenderer;
    public Animator Animator;
    [SerializeField] private Transform _aimTargetTransform;
    private SpriteRenderer _aimRenderer = null;

    private Vector2 _inputDirection = Vector2.zero;
    private Vector2 _targetDirection = Vector2.zero;

    private bool _attackButton1Down = false;
    private bool _attackButton2Down = false;

    private Vector2 _velocity = Vector2.zero;
    private float _attackCooldownRemaining = 0f;

    private float _moveSpeedModifier = 1f;
    private float _attackCooldownModifier = 1f;

    private InputManager _input;

    // Summons
    public int SummonLevel = 0;
    public float SummonCooldownFactor = 1f;
    private float _lv1SummonTimer = 0.5f;
    private float _lv2SummonTimer = 0.5f;
    private float _lv3SummonTimer = 0.5f;

    void Awake ()
    {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
        RangedAttack = GetComponent<RangedAttack>();
        MeleeAttack = GetComponent<MeleeAttack>();
        LevelingSystem = GetComponent<PlayerLeveling>();
        Summoner = GetComponent<Summoner>();

        _aimRenderer = _aimTargetTransform.GetComponent<SpriteRenderer>();
    }

    private void Start ()
    {
        UnitManager.FriendlyUnits.Add(transform);
        _input = InputManager.Instance;
    }

    void Update ()
    {
        if (UIManager.Paused || UIManager.GameOver) { return; }

        ReadInput();
        ApplyMovement();
        ApplyActions();
        ShowTarget();
        UpdateTimers();
    }

    private void OnDestroy ()
    {
        //RangedAttackEvent.RemoveAllListeners();
        //MeleeAttackEvent.RemoveAllListeners();
        UnitManager.FriendlyUnits.Remove(transform);
        Instance = null;
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_targetDirection);
    }

    // Ugly wrapper function until Static calls from UnityEvents are allowed
    public void OnDamageTaken() 
    {   
        AudioManager.PlaySFX(SFXType.PlayerTakeDamage);
    }

    private void ReadInput ()
    {
        _inputDirection = _input.MoveAction.ReadValue<Vector2>();

        var moving = _inputDirection != Vector2.zero;
        Animator.SetBool("moving", moving);

        if (_input.UsingMouseAndKeyboard)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(_input.AimAction.ReadValue<Vector2>());
            mouseWorldPosition.z = 0f;
            Vector3 origin = transform.position + (Vector3)RangedAttack.AbsoluteSpawnOffset;
            _targetDirection = (mouseWorldPosition - origin).normalized;
        }
        else
        {
            _targetDirection = _input.AimAction.ReadValue<Vector2>();
        }

        if (!_input.UsingMouseAndKeyboard || !EventSystem.current.IsPointerOverGameObject())
        {
            _attackButton1Down = _input.Fire1Action.ReadValue<float>() > 0.5f;
            _attackButton2Down = _input.Fire2Action.ReadValue<float>() > 0.5f;
        }
        else
        {
            _attackButton1Down = false;
            _attackButton2Down = false;
        }
    }

    private void ApplyMovement ()
    {
        _velocity = _inputDirection * _baseMoveSpeed * _moveSpeedModifier;        
        if (_inputDirection.x < 0 && !SpriteRenderer.flipX) 
        {
            SpriteRenderer.flipX = true;
        } 
        else if (_inputDirection.x > 0 && SpriteRenderer.flipX)
        {
            SpriteRenderer.flipX = false;
        }
        _rigidBody.velocity = _velocity;
    }

    private void ApplyActions()
    {
        if (_attackButton1Down)
        {
            if (_attackCooldownRemaining <= 0f)
            {
                //RangedAttackEvent?.Invoke(_targetDirection);
                RangedAttack.Fire(_targetDirection);
                _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
            }
        }

        if (_attackButton2Down)
        {
            if (_attackCooldownRemaining <= 0f)
            {
                //MeleeAttackEvent?.Invoke(_targetDirection);
                MeleeAttack.Fire(_targetDirection);
                _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
            }
        }
        
        if (SummonLevel >= 3 && _lv3SummonTimer <= 0f) 
        {   
            Summoner.Summon(2);
            _lv3SummonTimer = _baseLv3SummonSpeed * SummonCooldownFactor * Random.Range(0.9f, 1.1f);
        }
        if (SummonLevel >= 2 && _lv2SummonTimer <= 0f) 
        {   
            Summoner.Summon(1);
            _lv2SummonTimer = _baseLv2SummonSpeed * SummonCooldownFactor * Random.Range(0.9f, 1.1f);
        }
        if (SummonLevel >= 1 && _lv1SummonTimer <= 0f) 
        {   
            Summoner.Summon(0);
            _lv1SummonTimer = _baseLv1SummonSpeed * SummonCooldownFactor * Random.Range(0.9f, 1.1f);
        }
    }

    private void ShowTarget ()
    {
        if (_input.UsingMouseAndKeyboard) 
        {
            _aimRenderer.color = Color.clear;
        }
        else 
        { 
            if (_targetDirection.sqrMagnitude > 0.01f)
            {
                _aimRenderer.color = Color.white;
                _aimTargetTransform.localPosition = RangedAttack.AbsoluteSpawnOffset + _targetDirection * 2f;
            }
            else
            {
                _aimRenderer.color = Color.clear;
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
        if (SummonLevel >= 1 && _lv1SummonTimer > 0f) 
        {
            _lv1SummonTimer -= Time.deltaTime;
        }
        if (SummonLevel >= 2 && _lv2SummonTimer > 0f) 
        {
            _lv2SummonTimer -= Time.deltaTime;
        }
        if (SummonLevel >= 3 && _lv3SummonTimer > 0f) 
        {
            _lv3SummonTimer -= Time.deltaTime;
        }
    }
}
