using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Projectile _projectile = null;
    [SerializeField] private float _spawnOffset = 0.5f;
    [SerializeField] private Transform _projectileContainer = null;

    private List<Projectile> _projectilePool = new List<Projectile>();

    public int damage = 5;
    public float projectileSpeed = 7f;
    public int piercingAmount = 0;
    public bool homing = false;

    private static float _timeOutTime = 3f;

    private void Update ()
    {
        foreach (var item in _projectilePool)
        {
            if (!item.isActiveAndEnabled) { continue; }

            // timeout
            if (Time.time - item.creationTime > _timeOutTime)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if (homing)
            {
                // todo: homing logic
                item.SetDirection(Vector2.down);
            }

            item.transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
        }
    }

    private void OnDestroy ()
    {
        foreach (var item in _projectilePool)
        {
            item.DamagableHit.RemoveListener(OnDamagableHit);
            Destroy(item.gameObject);
        }
    }

    public void Fire (Vector2 direction)
    {
        if (_projectile == null) { return; }

        Projectile projectileUsed = null;
        foreach (var item in _projectilePool)
        {
            if (!item.isActiveAndEnabled)
            {
                projectileUsed = item;
                projectileUsed.gameObject.SetActive(true);
                break;
            }
        }

        if (projectileUsed == null)
        {
            projectileUsed = Instantiate(_projectile);
            _projectilePool.Add(projectileUsed);
            projectileUsed.DamagableHit.AddListener(OnDamagableHit);
            if (_projectileContainer != null)
            {
                projectileUsed.transform.parent = _projectileContainer;
            }
        }

        projectileUsed.creationTime = Time.time;
        projectileUsed.remainingPierces = piercingAmount;
        projectileUsed.transform.position = transform.position + (Vector3)direction * _spawnOffset;
        projectileUsed.SetDirection(direction);
    }

    private void OnDamagableHit (Damagable damagable)
    {
        damagable.Hit(damage);
    }
}
