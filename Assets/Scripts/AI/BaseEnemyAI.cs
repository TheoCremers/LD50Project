using System.Collections;
using UnityEngine;

public abstract class BaseEnemyAI : BaseUnitAI 
{
    [SerializeField]
    private int ExpWorth;

    protected EnemyCombatState _combatState = EnemyCombatState.Patrol;

    protected EnemyPatrolState _patrolState = EnemyPatrolState.Idle;

    [SerializeField]
    protected float _wanderSpeed;

    protected float _patrolTime;

    protected float _seekTime;

    protected float _distanceToTarget;

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
            UpdateTargets();
        } 

        switch (_combatState) 
        {
            case (EnemyCombatState.Patrol): 
                PatrolBehavior();
                break;
            case (EnemyCombatState.Agro):
                AgroBehavior();
                break;
            case (EnemyCombatState.Dead):
                break;
        }

        UpdateTimers();
        base.Update();
    }

    private void PatrolBehavior() 
    {
        // Transitions
        if (_target != null && _distanceToTarget < _currentAgroRange)
        {
            _combatState = EnemyCombatState.Agro;
        }
        else
        {
            switch (_patrolState) 
            {
                case (EnemyPatrolState.Idle): 
                    IdleBehavior();
                    break;
                case (EnemyPatrolState.Roam):
                    RoamBehavior();
                    break;
            }
        }
    }

    private void EnterIdleBehavior()
    {
        _patrolTime = Random.Range(1.0f, 3.0f);
    }

    private void IdleBehavior()
    {
        // Transitions
        if (_patrolTime < 0) 
        {
            EnterRoamBehavior();
            _patrolState = EnemyPatrolState.Roam;
        }
        // Actions
        else
        {
            // Stand Idle
        } 
    }

    private void EnterRoamBehavior()
    {
        // Start moving in a random direction
        _patrolTime = Random.Range(3.0f, 7.0f);
        var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var randomSpeedMod = Random.Range(0.3f, 0.4f);
        _facingDirection = new Vector2(Mathf.Cos(angle) * randomSpeedMod, Mathf.Sin(angle) * randomSpeedMod);
    }

    private void RoamBehavior()
    {
        // Transitions
        if (_patrolTime < 0) 
        {
            EnterIdleBehavior();
            _patrolState = EnemyPatrolState.Idle;
        }
        // Actions
        else
        {
            RigidBody.velocity = _facingDirection * _wanderSpeed;            
        }
    }

    // Force agro when hit while idle
    public void OnHit() 
    {
        if (_combatState == EnemyCombatState.Patrol) 
        {
            _combatState = EnemyCombatState.Agro;
            _currentAgroRange = 99f;
            _target = UnitManager.GetClosestFriendly(transform.position);
        }
    }

    protected virtual void DestroyIfTooFar(float maxDistance)
    {
        if (_distanceToTarget > maxDistance) 
        {
            Destroy(gameObject);
        }
    }

    public void DropExp()
    {
        ExperienceOrbManager.Instance.SpawnExperienceOrbs(transform.position, Mathf.CeilToInt(ExpWorth));
    }

    protected abstract void UpdateTargets();

    protected abstract void AgroBehavior();   

    public void TriggerDeathAnimation ()
    {
        _combatState = EnemyCombatState.Dead;

        // Stop Movement
        RigidBody.velocity = Vector3.zero;

        // turn off all colliders
        foreach (var collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        // stop sprite animation
        if (Sprite.TryGetComponent(out Animator animator))
        {
            animator.enabled = false;
        }

        // fade sprite out
        StartCoroutine(FadeOutAndDestroy());
    }

    protected virtual void UpdateTimers()
    {
        _seekTime -= Time.deltaTime;
        _patrolTime -= Time.deltaTime;
    } 
}
