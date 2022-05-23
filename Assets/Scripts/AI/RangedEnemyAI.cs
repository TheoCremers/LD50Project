using UnityEngine;

public class RangedEnemyAI : BaseEnemyAI 
{   
    [SerializeField] 
    protected float _deadzone;

    [SerializeField] 
    protected float _range;

    private EnemyAgroState _agroState = EnemyAgroState.Chase;

    protected RangedAttack _rangedAttack;
    protected float _attackCooldownRemaining = 0f;

    [SerializeField] 
    private float _baseAttackCooldown = 1f;

    protected override void Start()
    {
        base.Start();
        _rangedAttack = GetComponent<RangedAttack>();
    }

    protected override void Update()
    {                        
        base.Update();
        UpdateTimers();
    }

    protected override void UpdateTargets()
    {
        // There should always be a friendly target left, else it's game over
        _target = UnitManager.GetClosestFriendly(transform.position);
        _distanceToTarget = Vector2.Distance(_target.position, transform.position);

        // If enemy is too far away Destroy
        DestroyIfTooFar(_combatState == EnemyCombatState.Patrol ? 25f : 40f);
        _seekTime = 0.2f;
    }

    protected override void AgroBehavior()
    {
        // Transitions
        if (_target == null) 
        {
            _combatState = EnemyCombatState.Patrol;
            _currentAgroRange = _agroRange;
        }  
        // Actions
        else
        {
            switch (_agroState)
            {
                case EnemyAgroState.Chase:
                    ChaseBehavior();
                    break;
                case EnemyAgroState.Attack:
                    AttackBehavior();
                    break;                
                case EnemyAgroState.TakeDistance:
                    TakeDistanceBehavior();
                    break;         
            }
        }        
    }

    protected virtual void ChaseBehavior()
    {
        // Transitions
        if (_distanceToTarget < _deadzone) 
        {
            _agroState = EnemyAgroState.TakeDistance;
        } 
        else if (_distanceToTarget <= _range)
        {
            _agroState = EnemyAgroState.Attack;
        } 
        // Actions
        else 
        {
            BodyAnimator.SetBool("moving", true);
            var relativeVector = _target.position - transform.position;
            _facingDirection = relativeVector.normalized;
            RigidBody.velocity = _facingDirection * _moveSpeed;
        }
    }

    protected virtual void AttackBehavior()
    {
        // Transitions
        if (_distanceToTarget < _deadzone) 
        {
            _agroState = EnemyAgroState.TakeDistance;
        } 
        else if (_distanceToTarget > _range)
        {
            _agroState = EnemyAgroState.Chase;
        } 
        // Actions
        else
        {
            if (_attackCooldownRemaining <= 0f) 
            {
                BodyAnimator.SetBool("moving", false);
                RigidBody.velocity = Vector2.zero;
                var relativeVector = _target.position - transform.position;
                _facingDirection = relativeVector.normalized;
                _rangedAttack.Fire(_facingDirection);
                _attackCooldownRemaining = _baseAttackCooldown;
            }
        }
    }

    protected virtual void TakeDistanceBehavior()
    {
        // Transitions
        if (_distanceToTarget > _range)
        {
            _agroState = EnemyAgroState.Chase;
        } 
        else if (_distanceToTarget > _deadzone) 
        {
            _agroState = EnemyAgroState.Attack;
        } 
        // Actions
        else 
        {
            BodyAnimator.SetBool("moving", true);
            var relativeVector = _target.position - transform.position;
            _facingDirection = relativeVector.normalized;
            RigidBody.velocity = -(_facingDirection * _moveSpeed);
        }
    }

    protected override void UpdateTimers()
    {            
        base.UpdateTimers();
        _attackCooldownRemaining -= Time.deltaTime;
    }        
}

