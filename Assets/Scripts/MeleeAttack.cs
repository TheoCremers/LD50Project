using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _swingOffset = 1.0f;
    [SerializeField] private Transform _swingTransform = null;
    [SerializeField] private float _attackTime = 0.3f;

    private bool _isAttacking = false;
    private SpriteRenderer _swingSprite = null;
    private Collider2D _swingCollider = null;

    private void Start ()
    {
        if (_swingTransform == null)
        {
            Debug.LogError("No Swing transform in MeleeAttack!");
            return;
        }

        _swingSprite = _swingTransform.GetComponent<SpriteRenderer>();
        _swingCollider = _swingTransform.GetComponent<Collider2D>();

        _swingCollider.enabled = false;
        _swingSprite.color = Color.clear;
    }

    public void Fire (Vector2 direction)
    {
        if (!_isAttacking)
        {
            _swingTransform.position = transform.position + (Vector3) direction * _swingOffset;
            _swingTransform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            StartCoroutine(AttackAnimation());
        }
    }

    private void OnDestroy ()
    {
        StopAllCoroutines();
    }

    private IEnumerator AttackAnimation ()
    {
        _isAttacking = true;
        _swingCollider.enabled = true;
        float timeElapsed = 0f;

        while (timeElapsed < _attackTime * 0.5f)
        {
            _swingSprite.color = Color.Lerp(Color.clear, Color.white, timeElapsed * 2f / _attackTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        while (timeElapsed < _attackTime * 0.5f)
        {
            _swingSprite.color = Color.Lerp(Color.clear, Color.white, timeElapsed * 2f / _attackTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _swingSprite.color = Color.clear;
        _swingCollider.enabled = false;
        _isAttacking = false;
    }
}
