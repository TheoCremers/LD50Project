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
        // Transitions
        if (_distanceToTarget < _meleeRange)
        {
            _agroState = FamiliarAgroState.Attack;
        } 
        // Actions
        else 
        {
            var relativeVector = _target.position - transform.position;
            _moveDirection = relativeVector.normalized;
            RigidBody.velocity = _moveDirection * _moveSpeed;
        }
    }

    protected virtual void AttackBehavior()
    {
        // Transitions
        if (_distanceToTarget >= _meleeRange)
        {
            _agroState = FamiliarAgroState.Chase;
        } 
        // Actions
        else
        {
            if (_attackCooldownRemaining <= 0f) 
            {
                RigidBody.velocity = Vector2.zero;
                _meleeAttack.Fire(_moveDirection);
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


