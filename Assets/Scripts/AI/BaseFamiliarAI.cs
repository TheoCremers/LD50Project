using UnityEngine;

public abstract class BaseFamiliarAI : BaseUnitAI 
{   
    protected FamiliarCombatState _combatState = FamiliarCombatState.Following;

    protected FamiliarAgroState _agroState = FamiliarAgroState.Chase;

    protected float _seekTime;

    protected float _distanceToTarget;
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
            UpdateTargets();
        }

        switch (_combatState) 
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

    private void UpdateTargets()
    {
        _target = UnitManager.GetClosestEnemy(transform.position);
        if (_target != null) 
        {
            _distanceToTarget = Vector2.Distance(_target.position, transform.position);
        }
        _distanceToMaster = Vector2.Distance(_master.position, transform.position);
        _seekTime = 0.2f;
    }

    private void FollowBehavior() 
    {
        // Transitions
        if (_target != null && _distanceToTarget < _currentAgroRange)
        {
            _combatState = FamiliarCombatState.Agro;
        }
        // Actions
        else
        {
            if (_distanceToMaster > 1f) {
                var relativeVector = _master.position - transform.position;
                _moveDirection = relativeVector.normalized;
                RigidBody.velocity = _moveDirection * _moveSpeed;
            }
        }
    }

    // Force agro when hit while idle
    public void OnHit() 
    {
        if (_combatState == FamiliarCombatState.Following) 
        {
            _combatState = FamiliarCombatState.Agro;
            _target = UnitManager.GetClosestEnemy(transform.position);
            _currentAgroRange = 99f;
        }
    }

    protected abstract void AgroBehavior();

    public void TriggerDeathAnimation ()
    {
        _combatState = FamiliarCombatState.Dead;

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
    } 
}