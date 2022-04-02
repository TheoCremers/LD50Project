using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private static float _timeOutTime = 3f;

    public float directionAngle = 0f;
    public int damage = 5;
    public float projectileSpeed = 7f;
    public bool homing = false;
    public int remainingPierces = 0;
    public float creationTime;

    public UnityEvent<Damagable> DamagableHit;

    private void Start ()
    {
        creationTime = Time.time;
    }

    private void Update ()
    {
        // timeout
        if (Time.time - creationTime > _timeOutTime)
        {
            Destroy(gameObject);
            return;
        }

        if (homing)
        {
            // todo: homing logic
            SetDirection(Vector2.down);
        }

        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
    }

    public void SetDirection (Vector2 direction) 
    {
        directionAngle = Vector2.SignedAngle(Vector2.up, direction);
        transform.eulerAngles = Vector3.forward * directionAngle;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            damagable.Hit(damage);

            if (remainingPierces > 0) { --remainingPierces; }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
