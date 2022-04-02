using UnityEngine;

public enum EnemyCombatState 
{
    Idle,
    Agro,
    Dead
}
public class BaseEnemyAI : MonoBehaviour 
{   
    [SerializeField]
    private int _collisionAttack;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _wanderSpeed;

    [SerializeField]
    private float _agroRange;

    private EnemyCombatState _state = EnemyCombatState.Idle;
    private float _wanderTime;
    private float _standTime;

    private Vector2 _moveDirection;    

    public SpriteRenderer Sprite;

    private Transform _player;

    public Rigidbody2D RigidBody;


    // Start is called before the first frame update
    internal void Start()     
    {       
        _player = PlayerController.Instance?.transform;
    }

    // Update is called once per frame
    internal void Update() 
    {
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

        // Visual
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_moveDirection.x < 0) 
        {
            Sprite.flipX = true;
        } 
        else if (_moveDirection.x > 0) 
        {
            Sprite.flipX = false;
        }
    }

    private void IdleBehavior() 
    {
        if (_player != null && Vector2.Distance(_player.position, transform.position) < _agroRange)
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

    private void AgroBehavior()
    {
        var relativeVector = _player.position - transform.position;
        _moveDirection = relativeVector.normalized;
        RigidBody.velocity = _moveDirection * _wanderSpeed;
    }
}
