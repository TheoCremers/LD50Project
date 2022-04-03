using UnityEngine;

namespace LD50.Scripts.AI 
{
    public class MeleeEnemyAI : BaseEnemyAI 
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

        protected override void UpdateTargetting()
        {
            // There should always be a friendly target left, else it's game over
            _target = UnitManager.GetClosestFriendly(transform.position);
            _distanceToTarget = Vector2.Distance(_target.position, transform.position);
            // If enemy is too far away Destroy
            DestroyIfTooFar(_state == EnemyCombatState.Idle ? 25f : 40f);
            _seekTime = 0.2f;
        }

        protected override void AgroBehavior()
        {
            if (_target == null) 
            {
                _state = EnemyCombatState.Idle;
                _currentAgroRange = _agroRange;
                return;
            }  
            // If close enough to target, swing
            //var distanceToTarget = Vector2.Distance(_target.position, transform.position);
            if (_distanceToTarget < _meleeRange)
            {
                if (_attackCooldownRemaining <= 0f) 
                {
                    RigidBody.velocity = Vector2.zero;
                    _meleeAttack.Fire(_moveDirection);
                    _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
                }
            } 
            // Move closer to target
            else 
            {
                var relativeVector = _target.position - transform.position;
                _moveDirection = relativeVector.normalized;
                RigidBody.velocity = _moveDirection * _moveSpeed;
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
}

