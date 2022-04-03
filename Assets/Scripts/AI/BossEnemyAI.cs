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

        private Vector2 _playerDirection;
        protected RangedAttack _rangedAttack;

        public Damagable HitpointData;
        
        [SerializeField] 
        private float _baseAttackCooldown = 1f;
        private float _attackCooldownModifier = 1f;

        [SerializeField]
        private float _distanceDifficultyModifier = 1f;

        private bool _phase4Transition = false;

        protected override void Start()
        {
            base.Start();
            _rangedAttack = GetComponent<RangedAttack>();
            _meleeAttack = GetComponent<MeleeAttack>();
        }

        protected override void UpdateTargetting()
        {
            // There should always be a friendly target left, else it's game over
            _target = UnitManager.GetClosestFriendly(transform.position);
            _distanceToTarget = Vector2.Distance(_target.position, transform.position);
            _distanceDifficultyModifier = 1f + (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) / 30f);

            var relativeVector = PlayerController.Instance.transform.position - transform.position;
            _playerDirection = relativeVector.normalized;

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
                Phase1Mechanics(); // 100-90%
            } 
            else if (HitpointData.HealthPercentage > 0.65f)
            {                
                Phase2Mechanics(); // 65-90%
            } 
            else if (HitpointData.HealthPercentage > 0.1f)
            {
                Phase3Mechanics(); // 70-10%
            } 
            else if (HitpointData.HealthPercentage > 0.03f)
            {
                Phase4Mechanics(); // (buffed) 100-40%
            }
            else 
            {
                Phase5Mechanics(); // (buffed) 40-0%
            }  
        }

        private void Phase2Mechanics()
        {
            // Boss will continue his Phase 1 mechanics, but also shoot a lot of piercing bullets
            Phase1Mechanics();

            _rangedAttack.Fire(_playerDirection);
            var quat = Quaternion.Euler(0f, 0f, 30f);
            _rangedAttack.Fire(quat * _moveDirection);
        }

        private void Phase3Mechanics()
        {
            // Boss will actively try to hinder the player by spawning deathzones in their path
        }

        private void Phase4Mechanics()
        {
            // Boss gets tankier, vastly increasing his effective health

            if (!_phase4Transition) 
            {
                HitpointData.MaxHealth = HitpointData.MaxHealth * 10;
                HitpointData.Health = HitpointData.Health * 10;
            }
        }

        private void Phase5Mechanics()
        {
            // Boss will go berserk and unleash a bullet hell 
        }

        private void Phase1Mechanics()
        {
            // Phase1: Boss simply acts as a strong melee enemy that scales with distance from player
            if (_target == null) 
            {
                _target = UnitManager.GetClosestFriendly(transform.position);
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

