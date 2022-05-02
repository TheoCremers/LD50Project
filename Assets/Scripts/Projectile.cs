using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public float Lifespan = 4f;
    public float DirectionAngle = 0f;
    public int Damage = 5;
    public float ProjectileSpeed = 7f;
    public bool Homing = false;
    public bool ChainLighting = false;
    public int RemainingPierces = 0;
    public float CreationTime;
    public ChainLightning ChainLightningTemplate = null;

    public SpriteRenderer Sprite;
    public ParticleSystem ParticalSystem;
    public Collider2D ProjectileCollider;

    public UnityEvent<Damagable> DamagableHit;

    private void Start ()
    {
        CreationTime = Time.time;
    }

    private void Update ()
    {
        // timeout
        if (Time.time - CreationTime > Lifespan)
        {
            DestroyAfterParticlesGone();
            return;
        }

        if (Homing)
        {
            // todo: homing logic
            SetDirection(Vector2.down);
        }

        transform.Translate(Vector3.up * ProjectileSpeed * Time.deltaTime);
    }

    public void SetDirection (Vector2 direction) 
    {
        DirectionAngle = Vector2.SignedAngle(Vector2.up, direction);
        transform.eulerAngles = Vector3.forward * DirectionAngle;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            damagable.Hit(Damage);

            if (ChainLighting) 
            {
                var lightning = Instantiate(ChainLightningTemplate);
                lightning.Activate(collision.transform.parent);
                lightning.Damage = Damage;
                lightning.MaxTargets = RemainingPierces + 1;
            }
            else if (RemainingPierces > 0) 
            {
                --RemainingPierces;
                return;
            }
        }
        //Destroy(gameObject);
        DestroyAfterParticlesGone();
    }

    private void DestroyAfterParticlesGone ()
    {
        ProjectileCollider.enabled = false;
        Sprite.enabled = false;
        ParticalSystem.Stop();
    }
}
