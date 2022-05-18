using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : AIState
{
    private float _distanceToMaster;

    public new BaseFamiliarModel Model;

    private float _seekTime = 0.2f;

    private Transform _master;
    //

    public override void Enter()
    {
        _master = PlayerController.Instance.transform;
    }

    public override void Logic()
    {
        if (_seekTime <= 0f) 
        {
            _distanceToMaster = Vector2.Distance(_master.position, transform.position);
        }
        if (_distanceToMaster > 1f) {
            Model   lativeVector = _master.position - transform.position;
            Entity._moveDirection = relativeVector.normalized;
            Entity.RigidBody.velocity = _moveDirection * _moveSpeed;
        }
        _seekTime -= Time.deltaTime;
    }
} 