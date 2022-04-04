using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swing : MonoBehaviour
{
    public UnityEvent<Damagable> DamagableHit;
    private SpriteRenderer _swingSprite = null;
    private Collider2D _swingCollider = null;
    private Animator _swingAnimator = null;

    private void Start ()
    {
        _swingSprite = GetComponent<SpriteRenderer>();
        _swingCollider = GetComponent<Collider2D>();
        _swingAnimator = GetComponent<Animator>();
        _swingSprite.enabled = false;
        _swingCollider.enabled = false;
        _swingAnimator.speed = 4f;
    }

    public void Activate()
    {
        _swingAnimator.SetTrigger("SwingTrigger");
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            DamagableHit?.Invoke(damagable);
        }
    }
}