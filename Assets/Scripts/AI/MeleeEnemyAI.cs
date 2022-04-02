using UnityEngine;

namespace LD50.Scripts.AI 
{
    public class MeleeEnemyAI : BaseEnemyAI 
    {   
        protected override void AgroBehavior()
        {
            var relativeVector = _target.position - transform.position;
            _moveDirection = relativeVector.normalized;
            RigidBody.velocity = _moveDirection * _wanderSpeed;
        }
    }
}

