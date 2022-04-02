using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private float _spawnOffset = 0.5f;

    [SerializeField] private Transform _projectileContainer = null;


    public void Fire (Vector2 direction)
    {
        if (_projectile != null)
        {
            Projectile newProj = Instantiate(_projectile);
            newProj.transform.position = transform.position + (Vector3)direction * _spawnOffset;
            newProj.directionAngle = Vector2.SignedAngle(Vector2.up, direction);
            if (_projectileContainer != null)
            {
                newProj.transform.parent = _projectileContainer;
            }
        }
    }
}
