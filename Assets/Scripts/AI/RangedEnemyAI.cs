using UnityEngine;

namespace LD50.Scripts.AI 
{
    public class RangedEnemyAI : BaseEnemyAI 
    {   
        [SerializeField] 
        protected float _deadzone;

        [SerializeField] 
        protected float _range;

        protected RangedAttack _rangedAttack;
        protected float _attackCooldownRemaining = 0f;

        [SerializeField] 
        private float _baseAttackCooldown = 1f;
        private float _attackCooldownModifier = 1f;

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

        protected override void AgroBehavior()
        {
            // TODO: Add other targets based on collider
            var distanceToTarget = Vector2.Distance(_target.position, transform.position);
            var relativeVector = _target.position - transform.position;
            _moveDirection = relativeVector.normalized;

            // If too close to player, flee
            if (distanceToTarget < _deadzone) {
                RigidBody.velocity = -(_moveDirection * _wanderSpeed);
            } 
            // If close enough to player, shoot
            else if (distanceToTarget < _range)
            {
                if (_attackCooldownRemaining <= 0f) 
                {
                    RigidBody.velocity = Vector2.zero;
                    _rangedAttack.Fire(_moveDirection);
                    _attackCooldownRemaining = _baseAttackCooldown * _attackCooldownModifier;
                }
            } 
            // If too far from player, move to a nice position
            else
            {
                RigidBody.velocity = _moveDirection * _wanderSpeed; 
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

