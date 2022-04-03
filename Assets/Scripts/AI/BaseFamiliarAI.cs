using UnityEngine;

namespace LD50.Scripts.AI
{
    public enum FamiliarCombatState 
    {
        Following,
        Agro,
        Dead
    }

    public abstract class BaseFamiliarAI : BaseUnitAI 
    {   
        [SerializeField]
        private float _agroRange;

        protected FamiliarCombatState _state = FamiliarCombatState.Following;

        [SerializeField]
        protected float _followSpeed;

        protected float _seekTime;

        private float _distanceToTarget;
        private float _distanceToMaster;

        private Transform _master;

        protected override void Start()
        {
            UnitManager.FriendlyUnits.Add(transform);
            _master = PlayerController.Instance.transform;
            base.Start();
        }

        protected virtual void OnDestroy()
        {
            UnitManager.FriendlyUnits.Remove(transform);
        }

        protected override void Update() 
        {
            if (_seekTime <= 0) 
            {
                _target = UnitManager.GetClosestEnemy(transform.position);
                if (_target != null) 
                {
                    _distanceToTarget = Vector2.Distance(_target.position, transform.position);
                }
                _distanceToMaster = Vector2.Distance(_master.position, transform.position);
                _seekTime = 0.2f;
            } 
            else 
            {
                _seekTime -= Time.deltaTime;
            }


            switch (_state) 
            {
                case (FamiliarCombatState.Following): 
                    FollowBehavior();
                    break;
                case (FamiliarCombatState.Agro):
                    AgroBehavior();
                    break;
                case (FamiliarCombatState.Dead):
                    break;
            }

            base.Update();
        }

        private void FollowBehavior() 
        {
            if (_target != null && _distanceToTarget < _agroRange)
            {
                _state = FamiliarCombatState.Agro;
            }
            else
            {
                // Follow the player around
                if (_distanceToMaster > 1f) {
                    var relativeVector = _master.position - transform.position;
                    _moveDirection = relativeVector.normalized;
                    RigidBody.velocity = _moveDirection * _moveSpeed;
                }
            }
        }


        protected abstract void AgroBehavior();
    }
}
