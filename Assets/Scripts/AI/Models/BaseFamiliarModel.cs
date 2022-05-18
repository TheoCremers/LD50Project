using System.Collections;
using UnityEngine;

public abstract class BaseFamiliarModel : BaseUnitModel 
{   
    [SerializeField]
    protected float _followSpeed;

    protected float _seekTime;

    internal float _distanceToTarget;
    internal float _distanceToMaster;

    internal Transform _master;

    protected float _fadeTime = 0.2f;

    protected override void Start()
    {
        // UnitManager.FriendlyUnits.Add(transform);
        // _master = PlayerController.Instance.transform;
        // base.Start();
    }

    protected virtual void OnDestroy()
    {
        // UnitManager.FriendlyUnits.Remove(transform);
    }

    protected override void Update() 
    {
        // if (_seekTime <= 0) 
        // {
        //     _target = UnitManager.GetClosestEnemy(transform.position);
        //     if (_target != null) 
        //     {
        //         _distanceToTarget = Vector2.Distance(_target.position, transform.position);
        //     }
        //     _distanceToMaster = Vector2.Distance(_master.position, transform.position);
        //     _seekTime = 0.2f;
        // } 
        // else 
        // {
        //     _seekTime -= Time.deltaTime;
        // }


        // switch (_state) 
        // {
        //     case (FamiliarCombatState.Following): 
        //         FollowBehavior();
        //         break;
        //     case (FamiliarCombatState.Agro):
        //         AgroBehavior();
        //         break;
        //     case (FamiliarCombatState.Dead):
        //         break;
        // }

        // base.Update();
    }

    private void FollowBehavior() 
    {
        // if (_target != null && _distanceToTarget < _currentAgroRange)
        // {
        //     _state = FamiliarCombatState.Agro;
        // }
        // else
        // {
        //     // Follow the player around
        //     if (_distanceToMaster > 1f) {
        //         var relativeVector = _master.position - transform.position;
        //         _moveDirection = relativeVector.normalized;
        //         RigidBody.velocity = _moveDirection * _moveSpeed;
        //     }
        // }
    }

    // Force agro when hit while idle
    public void OnHit() 
    {
        // if (_state == FamiliarCombatState.Following) 
        // {
        //     _state = FamiliarCombatState.Agro;
        //     _target = UnitManager.GetClosestEnemy(transform.position);
        //     _currentAgroRange = 99f;
        // }
    }

    protected abstract void AgroBehavior();

    public void TriggerDeathAnimation ()
    {
        // _state = FamiliarCombatState.Dead;

        // // Stop Movement
        // RigidBody.velocity = Vector3.zero;

        // // turn off all colliders
        // foreach (var collider in GetComponentsInChildren<Collider2D>())
        // {
        //     collider.enabled = false;
        // }

        // // stop sprite animation
        // if (Sprite.TryGetComponent(out Animator animator))
        // {
        //     animator.enabled = false;
        // }

        // // fade sprite out
        // StartCoroutine(FadeOutAndDestroy());
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
        ///Destroy(gameObject);
    }
}

