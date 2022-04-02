using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static float _timeOutTime = 3f;

    public int baseDamage = 10;
    public float baseSpeed = 7f;
    public int piercingAmount = 0;
    public float directionAngle = 0f;

    private float _creationTime;

    private void Start ()
    {
        _creationTime = Time.time;
    }

    private void Update ()
    {
        // timeout
        if (Time.time - _creationTime > _timeOutTime)
        {
            Destroy(gameObject);
            return;
        }

        transform.eulerAngles = Vector3.forward * directionAngle;
        transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damageable))
        {
            damageable.Hit(baseDamage);

            if (piercingAmount > 0) { --piercingAmount; }
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
