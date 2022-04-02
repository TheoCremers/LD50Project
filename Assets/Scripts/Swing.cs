using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swing : MonoBehaviour
{
    public UnityEvent<Damagable> DamagableHit;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            DamagableHit?.Invoke(damagable);
        }
    }
}