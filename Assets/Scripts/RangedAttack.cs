using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private float _directionalSpawnOffset = 0.5f;
    [SerializeField] private Transform _projectileContainer = null;

    public int Damage = 5;
    public float ProjectileSpeed = 7f;
    public int PiercingAmount = 0;
    public bool Homing = false;
    public bool ChainLightning = false;
    public Vector2 AbsoluteSpawnOffset = Vector2.zero;

    public float BulletLifespan = 4f;
    public Color BulletColor;

    public SFXType SFX = SFXType.None;

    public void Start()
    {   
        BulletColor = _projectile.Sprite.color;
    }

    public void Fire (Vector2 direction)
    {
        if (_projectile == null) { return; }

        Projectile newProjectile = Instantiate(_projectile);
        if (_projectileContainer != null)
        {
            newProjectile.transform.parent = _projectileContainer;
        }

        newProjectile.Damage = Damage;
        newProjectile.ProjectileSpeed = ProjectileSpeed;
        newProjectile.RemainingPierces = PiercingAmount;
        newProjectile.Homing = Homing;
        newProjectile.ChainLighting = ChainLightning;
        newProjectile.Lifespan = BulletLifespan;
        newProjectile.Sprite.color = BulletColor;
        newProjectile.transform.position = transform.position + (Vector3)(AbsoluteSpawnOffset + direction * _directionalSpawnOffset);
        newProjectile.SetDirection(direction);

        AudioManager.PlaySFXVariation(SFX);
    }
}
