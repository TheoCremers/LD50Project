using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] public Projectile Projectile = null;
    [SerializeField] private float _directionalSpawnOffset = 0.5f;
    [SerializeField] private Vector3 _absoluteSpawnOffset = Vector2.zero;
    [SerializeField] private Transform _projectileContainer = null;

    public int damage = 5;
    public float projectileSpeed = 7f;
    public int piercingAmount = 0;
    public bool homing = false;
    public bool chainLightning = false;

    public float BulletLifespan = 4f;
    public Color BulletColor;

    public void Start()
    {   
        BulletColor = Projectile.Sprite.color;
    }

    public void Fire (Vector2 direction)
    {
        if (Projectile == null) { return; }

        Projectile newProjectile = Instantiate(Projectile);
        if (_projectileContainer != null)
        {
            newProjectile.transform.parent = _projectileContainer;
        }

        newProjectile.damage = damage;
        newProjectile.projectileSpeed = projectileSpeed;
        newProjectile.remainingPierces = piercingAmount;
        newProjectile.homing = homing;
        newProjectile.chainLighting = chainLightning;
        newProjectile.Lifespan = BulletLifespan;
        newProjectile.Sprite.color = BulletColor;
        newProjectile.transform.position = _absoluteSpawnOffset + transform.position + (Vector3)direction * _directionalSpawnOffset;
        newProjectile.SetDirection(direction);
    }
}
