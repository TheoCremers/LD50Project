using System.Collections;
using UnityEngine;

public enum EnemyCombatState 
{
    Idle,
    Agro,
    Dead
}

public abstract class BaseEnemyAI : BaseUnitAI 
{
    [SerializeField]
    private int ExpWorth;

    protected float _expMultiplier = 1.8f;

    protected EnemyCombatState _state = EnemyCombatState.Idle;

    [SerializeField]
    protected float _wanderSpeed;

    protected float _wanderTime;
    protected float _standTime;

    protected float _seekTime;

    protected float _distanceToTarget;

    protected float _fadeTime = 0.2f;

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
            UpdateTargetting();
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
        if (_target != null && _distanceToTarget < _currentAgroRange)
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

    // Force agro when hit while idle
    public void OnHit() 
    {
        if (_state == EnemyCombatState.Idle) 
        {
            _state = EnemyCombatState.Agro;
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
        ExperienceOrbManager.Instance.SpawnExperienceOrbs(transform.position, Mathf.CeilToInt(ExpWorth * _expMultiplier));
    }

    protected abstract void UpdateTargetting();

    protected abstract void AgroBehavior();

    public void TriggerDeathAnimation ()
    {
        _state = EnemyCombatState.Dead;

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

    private IEnumerator FadeOutAndDestroy ()
    {
        Color initialColor = Sprite.color;
        float t = 0f;
        while (t < _fadeTime * 0.5f)
        {
            t += Time.deltaTime;
            Sprite.color = Color.Lerp(initialColor, Color.black, t * 2f / _fadeTime);
            yield return null;
        }
        t = 0f;
        while (t < _fadeTime * 0.5f)
        {
            t += Time.deltaTime;
            Sprite.color = Color.Lerp(Color.black, Color.clear, t * 2f / _fadeTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
