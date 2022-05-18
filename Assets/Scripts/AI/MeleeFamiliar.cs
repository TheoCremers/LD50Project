    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeFamiliar : AIStateMachine 
{   
    public MeleeFamiliarModel Model;

    // States
    public Follow Follow;

    public Chase Chase;


    protected float _seekTime;

    protected Transform _target;

    protected float _distanceToTarget;
    private float _distanceToMaster;
    
    public float AgroRange;

    protected float _currentAgroRange;

    protected Vector2 _moveDirection;    

    public SpriteRenderer Sprite;
    
    public Rigidbody2D RigidBody;

    protected virtual void Awake()
    {
        // Pass core reference down to statetree
        var states = GetComponentsInChildren<AIState>();
        foreach (var s in states) {
            s.Model = Model;
        }
        //_currentAgroRange = AgroRange;
    }

    private void Update()
    {
        // if (_seekTime <= 0) 
        // {
        //     _target = UnitManager.GetClosestEnemy(transform.position);
        //     if (_target != null) 
        //     {
        //         _distanceToTarget = Vector2.Distance(_target.position, transform.position);
        //     }
        //     //_distanceToMaster = Vector2.Distance(_master.position, transform.position);
        //     _seekTime = 0.2f;
        // } 
        // _seekTime -= Time.deltaTime;        

        // if (_target != null && _distanceToTarget < _currentAgroRange)
        // {
        //     SetState(Chase);
        // } 
        // else
        // {
        //     SetState(Follow);
        // }

        CurrentState.Logic();
    }
}