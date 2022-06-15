using UnityEngine;

public class BossEnemyAI : BaseEnemyAI 
{   
    public static BossEnemyAI Instance;

    [SerializeField]
    protected float _meleeRange = 1f; 

    protected float _meleeAttackCooldownRemaining = 0f;
    protected float _rangedAttackCooldownRemaining = 0f;
    protected float _aoeAttackCooldownRemaining = 0f;

    [SerializeField]
    private AoeEffect _aoeEffect = null;

    protected MeleeAttack _meleeAttack;

    [SerializeField]

    private float _moveSpeedModifier = 1f;
    private float _distanceToPlayer;
    private Vector2 _playerDirection;
    private float _sprayOfset;
    protected RangedAttack _rangedAttack;

    public Damagable HitpointData;
    
    [SerializeField] 
    private float _baseAttackCooldown = 1f;

    [SerializeField]
    private float _distanceDifficultyModifier = 1f;

    private EnemyAgroState _agroState = EnemyAgroState.Chase;

    private BossPhaseState _phaseState = BossPhaseState.Phase1;

    // for attack detection
    [SerializeField]
    private CapsuleCollider2D _hitBoxCollider;
    private Vector2 _meleeCapsuleSize;

    protected void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        _rangedAttack = GetComponent<RangedAttack>();
        _meleeAttack = GetComponent<MeleeAttack>();

        _meleeCapsuleSize = _hitBoxCollider.size;
        _meleeCapsuleSize.x += _meleeRange;
        _meleeCapsuleSize.y += _meleeRange;
    }

    protected override void UpdateTargets()
    {
        // There should always be a friendly target left, else it's game over
        _target = UnitManager.GetClosestFriendly(transform.position);
        _distanceToTarget = Vector2.Distance(_target.position, transform.position);
        _distanceToPlayer = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        _distanceDifficultyModifier = 1f + (_distanceToPlayer / 10f);

        var relativeVector = PlayerController.Instance.transform.position - transform.position;
        _playerDirection = relativeVector.normalized;

        _seekTime = 0.2f;
    }

    protected override void Update()
    {                        
        base.Update();
        UpdateTimers();
    }

    protected override void AgroBehavior()
    {
        // Boss does not have transitions and is always agressive
        // Actions        
        if (_target == null)
        {
            _target = UnitManager.GetClosestFriendly(transform.position);
        }

        // Chase and melee attack
        switch (_agroState)
        {
            case EnemyAgroState.Chase:
                ChaseBehavior();
                break;
            case EnemyAgroState.Attack:
                AttackBehavior();
                break;                
        }

        // Special phase based abilities
        switch (_phaseState)
        {
            case BossPhaseState.Phase1:
                Phase1Behavior();
                break;
            case BossPhaseState.Phase2:
                Phase2Behavior();
                break;
            case BossPhaseState.Phase3:
                Phase3Behavior();
                break;       
            case BossPhaseState.Phase4:
                Phase4Behavior();
                break;
            case BossPhaseState.Phase5:
                Phase5Behavior();
                break;               
        }        
    }

    private void ChaseBehavior()
    {
        // Any targets within melee range?
        Collider2D targetCollider = Physics2D.OverlapCapsule(transform.position, _meleeCapsuleSize, _hitBoxCollider.direction, 0f, Masks.PlayerHitBox);

        // Transitions
        if (targetCollider != null)
        {
            _agroState = EnemyAgroState.Attack;
            _target = targetCollider.transform;
        }
        // Actions
        else
        {
            var relativeVector = _target.position - transform.position;
            _facingDirection = relativeVector.normalized;
            RigidBody.velocity = _facingDirection * _moveSpeed * _moveSpeedModifier * _distanceDifficultyModifier;
        }
    }

    private void AttackBehavior()
    {
        // Any targets within melee range?
        Collider2D targetCollider = Physics2D.OverlapCapsule(transform.position, _meleeCapsuleSize, _hitBoxCollider.direction, 0f, Masks.PlayerHitBox);

        // Transitions
        if (targetCollider == null)
        {
            _agroState = EnemyAgroState.Chase;
        }
        // Actions
        else
        {
            if (_meleeAttackCooldownRemaining <= 0f) 
            {
                _target = targetCollider.transform;
                var relativeVector = _target.position - transform.position;
                _facingDirection = relativeVector.normalized;
                RigidBody.velocity = Vector2.zero;
                _meleeAttack.Fire(_facingDirection);
                _meleeAttackCooldownRemaining = _baseAttackCooldown / _distanceDifficultyModifier;
            }
        }
    }

    private void Phase1Behavior()
    {
        // Transitions
        if (HitpointData.HealthPercentage <= 0.85f) 
        {
            _phaseState = BossPhaseState.Phase2;
        }
        // Actions
        // Boss does nothing extra in phase 1
    }

    private void Phase2Behavior()
    {
        // Transitions
        if (HitpointData.HealthPercentage <= 0.55f) 
        {
            EnterPhase3Behavior();
            _phaseState = BossPhaseState.Phase3;
        }
        // Actions
        else if (_rangedAttackCooldownRemaining <= 0f) 
        {
            SingleBulletRangedAttack(Random.Range(-40f, 40f), 0.5f);
        }    
    }

    private void SingleBulletRangedAttack(float angle, float interval)
    {
        var quat = Quaternion.Euler(0f, 0f, angle);
        _rangedAttack.Fire(quat * _facingDirection);                
        _rangedAttackCooldownRemaining = interval;
    }

    private void EnterPhase3Behavior()
    {
        _moveSpeedModifier = 1.6f;
        _meleeAttack.Damage = Mathf.CeilToInt(_meleeAttack.Damage * 2f);
        _rangedAttack.Damage = Mathf.CeilToInt(_rangedAttack.Damage * 1.5f);
        _rangedAttack.ProjectileSpeed = Mathf.CeilToInt(_rangedAttack.ProjectileSpeed * 1.2f);
    }

    private void Phase3Behavior()
    {
         // Transitions
        if (HitpointData.HealthPercentage <= 0.12f) 
        {
            EnterPhase4Behavior();
            _phaseState = BossPhaseState.Phase4;
        }
        // Actions
        else 
        {
            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                SingleBulletRangedAttack(Random.Range(-60f, 60f), 0.8f);
            }
            if (_aoeAttackCooldownRemaining <= 0f) 
            {
                PoisonAttack(Random.Range(-30f, 30f), 2.5f);
            }
        }
    }

    private void PoisonAttack(float angle, float interval)
    {
        var bossAoe = Instantiate(_aoeEffect);
        var quat = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));
        bossAoe.transform.position = PlayerController.Instance.transform.position + (quat * _playerDirection * Random.Range(2f, 6f)); 
        _aoeAttackCooldownRemaining = interval;
    }

    private void EnterPhase4Behavior()
    {
        // Boss gets tankier, vastly increasing his effective health
        HitpointData.MaxHealth = HitpointData.MaxHealth * 8;
        HitpointData.Health = HitpointData.Health * 8;
        _moveSpeedModifier = 2.2f;
        _meleeAttack.Damage = Mathf.CeilToInt(_meleeAttack.Damage * 2f);
        _rangedAttack.Damage = Mathf.CeilToInt(_rangedAttack.Damage * 2f);
        _rangedAttack.ProjectileSpeed = Mathf.CeilToInt(_rangedAttack.ProjectileSpeed * 1.2f);
    }

    private void Phase4Behavior()
    {
         // Transitions
        if (HitpointData.HealthPercentage <= 0.04f) 
        {
            EnterPhase5Behavior();
            _phaseState = BossPhaseState.Phase5;
        }
        // Actions
        else 
        {
            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                SingleBulletRangedAttack(Random.Range(-60f, 60f), 0.8f);
            }
            // Spawn a poison cloud
            if (_aoeAttackCooldownRemaining <= 0f) 
            {
                PoisonAttack(Random.Range(-30f, 30f), 2f);
            }
        }
    }

    private void EnterPhase5Behavior()
    {
        _rangedAttack.ProjectileSpeed = 11f;
        _rangedAttack.SFX = SFXType.None;
    }


    private void Phase5Behavior()
    {
        // Actions
        if (_rangedAttackCooldownRemaining <= 0f) 
        {
            MultiBulletRangedAttack();
        }
    }

    private void MultiBulletRangedAttack()
    {
        for (var i = 0; i < 13; i++)
        {
            var quat = Quaternion.Euler(0f, 0f, _sprayOfset + ((360f / 13) * i));
            _rangedAttack.Fire(quat * Vector2.right);
        }

        _sprayOfset += 2.5f;
        _rangedAttackCooldownRemaining = 0.3f;

        AudioManager.PlaySFXVariation(SFXType.BossRangedAttack1, gameObject);
    }

    protected override void UpdateTimers ()
    {
        base.UpdateTimers();
        _meleeAttackCooldownRemaining -= Time.deltaTime;    
        _rangedAttackCooldownRemaining -= Time.deltaTime;  
        _aoeAttackCooldownRemaining -= Time.deltaTime;        
    }

    protected override void OnDestroy()
    {
        if (_combatState == EnemyCombatState.Dead)
        {
            UIManager.Instance.TriggerGameOver(true);
        }
        Instance = null;
        base.OnDestroy();
    }
}


