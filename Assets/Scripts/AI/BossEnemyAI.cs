using UnityEngine;

namespace LD50.Scripts.AI 
{
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
        private float _attackCooldownModifier = 1f;

        [SerializeField]
        private float _distanceDifficultyModifier = 1f;

        private bool _phase4Transition = false;
        private bool _phase3Transition = false;


        protected void Awake()
        {
            Instance = this;
        }

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
            _distanceToPlayer = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
            _distanceDifficultyModifier = 1f + (_distanceToPlayer / 30f);

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
            else if (HitpointData.HealthPercentage > 0.12f)
            {
                Phase3Mechanics(); // 70-12%
            } 
            else if (HitpointData.HealthPercentage > 0.04f)
            {
                Phase4Mechanics(); // 12-5%
            }
            else 
            {
                Phase5Mechanics(); // 5-0%
            }  
        }

        private void Phase2Mechanics()
        {
            // Boss will continue his Phase 1 mechanics, but also shoot a lot of piercing bullets
            Phase1Mechanics();

            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                var quat = Quaternion.Euler(0f, 0f, Random.Range(-40f, 40f));
                _rangedAttack.Fire(quat * _moveDirection);
                
                _rangedAttackCooldownRemaining = 0.3f;
            }
        }

        private void Phase3Mechanics()
        {
            // Boss will actively try to hinder the player by spawning deathzones in their path
            // Boss also speeds up and shoots some projectiles
            if (!_phase3Transition) 
            {
                _moveSpeedModifier = 1.6f;
                _meleeAttack.Damage = Mathf.CeilToInt(_meleeAttack.Damage * 2f);
                _rangedAttack.damage = Mathf.CeilToInt(_rangedAttack.damage * 2f);
                _rangedAttack.projectileSpeed = Mathf.CeilToInt(_rangedAttack.projectileSpeed * 1.2f);
                _phase3Transition = true;
            }

            Phase1Mechanics();
            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                var quat = Quaternion.Euler(0f, 0f, Random.Range(-60f, 60f));
                _rangedAttack.Fire(quat * _moveDirection);
                _rangedAttackCooldownRemaining = 0.6f;
            }
            // Spawn a poison cloud
            if (_aoeAttackCooldownRemaining <= 0f) 
            {
                var bossAoe = Instantiate(_aoeEffect);
                var quat = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));
                bossAoe.transform.position = PlayerController.Instance.transform.position + (quat * _playerDirection * Random.Range(2f, 6f)); 
                _aoeAttackCooldownRemaining = 1.5f;
            }
        }

        private void Phase4Mechanics()
        {
            // Boss gets tankier, vastly increasing his effective health
            if (!_phase4Transition) 
            {
                HitpointData.MaxHealth = HitpointData.MaxHealth * 10;
                HitpointData.Health = HitpointData.Health * 10;
                _moveSpeedModifier = 2.2f;
                _meleeAttack.Damage = Mathf.CeilToInt(_meleeAttack.Damage * 2f);
                _rangedAttack.damage = Mathf.CeilToInt(_rangedAttack.damage * 2f);
                _rangedAttack.projectileSpeed = Mathf.CeilToInt(_rangedAttack.projectileSpeed * 1.2f);
                _phase4Transition = true;
            }

            Phase1Mechanics();
            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                var quat = Quaternion.Euler(0f, 0f, Random.Range(-60f, 60f));
                _rangedAttack.Fire(quat * _moveDirection);
                _rangedAttackCooldownRemaining = 0.6f;
            }
            // Spawn a poison cloud
            if (_aoeAttackCooldownRemaining <= 0f) 
            {
                var bossAoe = Instantiate(_aoeEffect);
                var quat = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));
                bossAoe.transform.position = PlayerController.Instance.transform.position + (quat * _moveDirection * Random.Range(2f, 6f)); 
                _aoeAttackCooldownRemaining = 1.5f;
            }
        }

        private void Phase5Mechanics()
        {
            // Boss will go berserk and unleash a bullet hell 
            Phase1Mechanics();
            _rangedAttack.projectileSpeed = 11f;

            if (_rangedAttackCooldownRemaining <= 0f) 
            {
                for (var i = 0; i < 13; i++)
                {
                    var quat = Quaternion.Euler(0f, 0f, _sprayOfset + ((360f / 13) * i));
                    _rangedAttack.Fire(quat * Vector2.right);
                }

                _sprayOfset += 2.5f;
                _rangedAttackCooldownRemaining = 0.15f;
            }
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
                if (_meleeAttackCooldownRemaining <= 0f) 
                {
                    RigidBody.velocity = Vector2.zero;
                    _meleeAttack.Fire(_moveDirection);
                    _meleeAttackCooldownRemaining = (_baseAttackCooldown * _attackCooldownModifier) / _distanceDifficultyModifier;
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
            if (_meleeAttackCooldownRemaining > 0f)
            {
                _meleeAttackCooldownRemaining -= Time.deltaTime;
                if (_meleeAttackCooldownRemaining < 0f)
                {
                    _meleeAttackCooldownRemaining = 0f;
                }
            } 
            if (_rangedAttackCooldownRemaining > 0f) 
            {
                _rangedAttackCooldownRemaining -= Time.deltaTime;
            }
            if (_aoeAttackCooldownRemaining > 0f) 
            {
                _aoeAttackCooldownRemaining -= Time.deltaTime;
            }
        }

        protected override void OnDestroy()
        {
            // TODO: Victory condition
            Instance = null;
            base.OnDestroy();
        }
    }
}

