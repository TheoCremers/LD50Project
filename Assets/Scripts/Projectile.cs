using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public float directionAngle = 0f;
    public int remainingPierces = 0;
    public float creationTime;

    public UnityEvent<Damagable> DamagableHit;

    public void SetDirection (Vector2 direction) 
    {
        directionAngle = Vector2.SignedAngle(Vector2.up, direction);
        transform.eulerAngles = Vector3.forward * directionAngle;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            DamagableHit?.Invoke(damagable);

            if (remainingPierces > 0) { --remainingPierces; }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
