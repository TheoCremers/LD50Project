using UnityEngine;

namespace LD50.Scripts.AI 
{
    public class BossEnemyAI : BaseEnemyAI 
    {   
        [SerializeField]
        protected float _meleeRange = 1f; 

        protected float _attackCooldownRemaining = 0f;


        protected MeleeAttack _meleeAttack;

        private float _moveSpeedModifier = 1f;

        public Damagable HitpointData;
        
        [SerializeField] 
        private float _baseAttackCooldown = 1f;
        private float _attackCooldownModifier = 1f;

        private float _distanceDifficultyModifier = 1f;

        private bool _phase4Transition = false;

        protected override void Start()
        {
            base.Start();
            _meleeAttack = GetComponent<MeleeAttack>();
        }

        protected override void UpdateTargetting()
        {
            // There should always be a friendly target left, else it's game over
            _target = UnitManager.GetClosestFriendly(transform.position);
            _distanceToTarget = Vector2.Distance(_target.position, transform.position);
            _seekTime = 0.2f;
        }

        protected override void Update()
        {                        
            base.Update();
            //_distanceToMaster = Vector2.Distance(_master.position, transform.position);
            UpdateTimers();
        }

        protected override void AgroBehavior()
        {
            if (HitpointData.HealthPercentage > 0.9f) 
            {
                Phase1Mechanics();
            } 
            else if (HitpointData.HealthPercentage > 0.7f)
            {

            } 
            else if (HitpointData.HealthPercentage > 0.5f)
            {

            } 
            else if (HitpointData.HealthPercentage > 0.1f)
            {

            } 
        }

        private void Phase2Mechanics()
        {

        }

        private void Phase3Mechanics()
        {
            
        }

        private void Phase4Mechanics()
        {
            if (!_phase4Transition) 
            {
                // Boss gets tankier, vastly increasing his effective health
                HitpointData.MaxHealth = HitpointData.MaxHealth * 8;
                HitpointData.Health = HitpointData.Health * 8;
            }
        }

        private void Phase1Mechanics()
        {
            if (_target == null) 
            {
                _state = EnemyCombatState.Idle;
                _currentAgroRange = _agroRange;
                return;
            }  

            // If close enough to target, swing
            var distanceToTarget = Vector2.Distance(_target.position, transform.position);
            if (distanceToTarget < _meleeRange)
            {
                if (_attackCooldownRemaining <= 0f) 
                {
                    RigidBody.velocity = Vector2.zero;
                    _meleeAttack.Fire(_moveDirection);
                    _attackCooldownRemaining = (_baseAttackCooldown * _attackCooldownModifier) / _distanceDifficultyModifier;
                }
            } 
            // Move closer to target
            else 
            {
                var relativeVector = _target.position - transform.position;
                _moveDirection = relativeVector.normalized;
                RigidBody.velocity = _moveDirection * _moveSpeed * _moveSpeedModifier * _distanceDifficultyModifier;
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

