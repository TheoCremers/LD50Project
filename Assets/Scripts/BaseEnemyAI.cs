using System.Collections;
using System.Collections.Generic;
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
    private float _movespeed;

    [SerializeField]
    private float _wanderspeed;

    private EnemyCombatState _state = EnemyCombatState.Idle;
    private float _wanderTime;
    private float _standTime;

    private Vector2 _moveDirection;    

    public SpriteRenderer _sprite;


    // Start is called before the first frame update
    internal void Start() 
    {        
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
            _sprite.flipX = true;
        } 
        else if (_moveDirection.x > 0) 
        {
            _sprite.flipX = false;
        }
    }

    private void IdleBehavior() 
    {
        // Check for state update
        // if is near player, state = agro
        // else
        {
            if (_wanderTime > 0) 
            {
                transform.Translate(_moveDirection * _wanderspeed * Time.deltaTime);
                _wanderTime -= Time.deltaTime;
                if (_wanderTime <= 0) {
                    _standTime = Random.Range(1.0f, 3.0f);
                }
            } 
            else if (_standTime > 0) 
            {
                _standTime -= Time.deltaTime;
            } 
            else 
            {            
                _wanderTime = Random.Range(3.0f, 7.0f);
                var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                var randomSpeedMod = Random.Range(0.3f, 0.4f);
                _moveDirection = new Vector2(Mathf.Cos(angle) * randomSpeedMod, Mathf.Sin(angle) * randomSpeedMod);
            }
        }
    }
}
