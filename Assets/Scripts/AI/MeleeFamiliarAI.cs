using UnityEngine;

public class MeleeFamiliarAI : BaseFamiliarAI 
{   
    [SerializeField]
    protected float _meleeRange = 1f; 

    protected float _attackCooldownRemaining = 0f;

    protected MeleeAttack _meleeAttack;
    
    [SerializeField] 
    private float _baseAttackCooldown = 1f;
    private float _attackCooldownModifier = 1f;

    // for attack detection
    [SerializeField]
    private CapsuleCollider2D _hitBoxCollider;
    private Vector2 _meleeCapsuleSize;

    protected override void Start()
    {
        base.Start();
        _meleeAttack = GetComponent<MeleeAttack>();

        _meleeCapsuleSize = _hitBoxCollider.size;
        _meleeCapsuleSize.x += _meleeRange;
        _meleeCapsuleSize.y += _meleeRange;
    }

    protected override void Update()
    {                        
        base.Update();
        UpdateTimers();
    }

    protected override void AgroBehavior()
    {
        // Transitions
        if (_target == null) 
        {
            _combatState = FamiliarCombatState.Following;
            _currentAgroRange = _agroRange;
            return;
        }  
        // Actions
        else
        {
            switch (_agroState)
            {
                case FamiliarAgroState.Chase:
                    ChaseBehavior();
                    break;
                case FamiliarAgroState.Attack:
                    AttackBehavior();
                    break;                
            }
        }
    }

    protected virtual void ChaseBehavior()
    {
        // Any targets within melee range?
        Collider2D targetCollider = Physics2D.OverlapCapsule(transform.position, _meleeCapsuleSize, _hitBoxCollider.direction, 0f, Masks.EnemyHitBoxes);

        // Transitions
        if (targetCollider != null)
        {
            _agroState = FamiliarAgroState.Attack;
            _target = targetCollider.transform;
        } 
        // Actions
        else 
        {
            var relativeVector = _target.position - transform.position;
            _facingDirection = relativeVector.normalized;
            RigidBody.velocity = _facingDirection * _moveSpeed;
        }
    }

    protected virtual void AttackBehavior()
    {
        // Any targets within melee range?
        Collider2D targetCollider = Physics2D.OverlapCapsule(transform.position, _meleeCapsuleSize, _hitBoxCollider.direction, 0f, Masks.EnemyHitBoxes);

        // Transitions
        if (targetCollider == null)
        {
            _agroState = FamiliarAgroState.Chase;
        }
        // Actions
        else
        {
            if (_attackCooldownRemaining <= 0f)
            {
                _target = targetCollider.transform;
                var relativeVector = _target.position - transform.position;
                _facingDirection = relativeVector.normalized;
                RigidBody.velocity = Vector2.zero;
                _meleeAttack.Fire(_facingDirection);
                _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
            }
        }
    }

    protected override void UpdateTimers ()
    {
        base.UpdateTimers();
        _attackCooldownRemaining -= Time.deltaTime;
    }  
}


