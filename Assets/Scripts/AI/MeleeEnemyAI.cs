using UnityEngine;


public class MeleeEnemyAI : BaseEnemyAI 
{   
    [SerializeField]
    protected float _meleeRange = 1f; 

    protected float _attackCooldownRemaining = 0f;

    private EnemyAgroState _agroState = EnemyAgroState.Chase;

    protected MeleeAttack _meleeAttack;
    
    [SerializeField] 
    private float _baseAttackCooldown = 1f;

    protected override void Start()
    {
        base.Start();
        _meleeAttack = GetComponent<MeleeAttack>();
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
            }
        } 
    }

    private void ChaseBehavior()
    {
        // Transitions
        if (_distanceToTarget < _meleeRange)
        {
            _agroState = EnemyAgroState.Attack;
        } 
        // Actions
        else 
        {
            var relativeVector = _target.position - transform.position;
            _facingDirection = relativeVector.normalized;
            RigidBody.velocity = _facingDirection * _moveSpeed;
        }
    }

    private void AttackBehavior()
    {
        // Transitions
        if (_distanceToTarget >= _meleeRange)
        {
            _agroState = EnemyAgroState.Chase;
        } 
        // Actions
        else
        {
            if (_attackCooldownRemaining <= 0f) 
            {
                var relativeVector = _target.position - transform.position;
                _facingDirection = relativeVector.normalized;
                RigidBody.velocity = Vector2.zero;
                _meleeAttack.Fire(_facingDirection);
                _attackCooldownRemaining = _baseAttackCooldown;
            }
        }
    }

    protected override void UpdateTimers ()
    {
        base.UpdateTimers();
        _attackCooldownRemaining -= Time.deltaTime;
    }  
}
