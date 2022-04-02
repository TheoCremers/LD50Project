using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private float _spawnOffset = 0.5f;
    [SerializeField] private Transform _projectileContainer = null;

    public int damage = 5;
    public float projectileSpeed = 7f;
    public int piercingAmount = 0;
    public bool homing = false;

    public void Fire (Vector2 direction)
    {
        if (_projectile == null) { return; }

        Projectile newProjectile = Instantiate(_projectile);
        if (_projectileContainer != null)
        {
            newProjectile.transform.parent = _projectileContainer;
        }

        newProjectile.damage = damage;
        newProjectile.projectileSpeed = projectileSpeed;
        newProjectile.remainingPierces = piercingAmount;
        newProjectile.homing = homing;
        newProjectile.transform.position = transform.position + (Vector3)direction * _spawnOffset;
        newProjectile.SetDirection(direction);
    }
}
