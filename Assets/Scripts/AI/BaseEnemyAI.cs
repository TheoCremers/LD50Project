using UnityEngine;

namespace LD50.Scripts.AI
{
    public enum EnemyCombatState 
    {
        Idle,
        Agro,
        Dead
    }

    public abstract class BaseEnemyAI : BaseUnitAI 
    {   
        [SerializeField]
        private float _agroRange;

        private EnemyCombatState _state = EnemyCombatState.Idle;
        [SerializeField]
        protected float _wanderSpeed;

        protected float _wanderTime;
        protected float _standTime;

        protected float _seekTime;

        protected override void Start()
        {
            UnitManager.EnemyUnits.Add(transform);
            base.Start();
        }

        protected virtual void OnDestroy()
        {
            UnitManager.EnemyUnits.Remove(transform);
        }

        protected override void Update() 
        {
            if (_seekTime <= 0) 
            {
                _target = UnitManager.GetClosestFriendly(transform.position);
                _seekTime = 0.2f;
            } 
            else 
            {
                _seekTime -= Time.deltaTime;
            }

            switch (_state) 
            {
                case (EnemyCombatState.Idle): 
                    IdleBehavior();
                    break;
                case (EnemyCombatState.Agro):
                    AgroBehavior();
                    break;
                case (EnemyCombatState.Dead):
                    break;
            }

            base.Update();
        }

        private void IdleBehavior() 
        {
            if (_target != null && Vector2.Distance(_target.position, transform.position) < _agroRange)
            {
                _state = EnemyCombatState.Agro;
            }
            else
            {
                if (_wanderTime > 0) 
                {
                    // Wander around
                    RigidBody.velocity = _moveDirection * _wanderSpeed;
                    _wanderTime -= Time.deltaTime;
                    if (_wanderTime <= 0) {
                        _standTime = Random.Range(1.0f, 3.0f);
                    }
                } 
                else if (_standTime > 0) 
                {
                    // Stand around, take a break
                    _standTime -= Time.deltaTime;
                } 
                else 
                {            
                    // Start moving in a random direction
                    _wanderTime = Random.Range(3.0f, 7.0f);
                    var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                    var randomSpeedMod = Random.Range(0.3f, 0.4f);
                    _moveDirection = new Vector2(Mathf.Cos(angle) * randomSpeedMod, Mathf.Sin(angle) * randomSpeedMod);
                }
            }
        }

        protected abstract void AgroBehavior();
    }
}
