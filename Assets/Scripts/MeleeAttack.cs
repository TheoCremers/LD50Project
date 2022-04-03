using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _swingOffset = 1.0f;
    [SerializeField] private Transform _swingTransform = null;
    [SerializeField] private float _attackTime = 0.3f;
    [SerializeField] private AoeEffect _aoeEffectTemplate = null;

    private bool _isAttacking = false;
    private SpriteRenderer _swingSprite = null;
    private Collider2D _swingCollider = null;
    private Swing _swing = null;
    private AoeEffect _activeAoeEffect = null;

    private Color _originalColor;

    public int Damage = 5;
    public bool LeavesAoe = false;

    private void Start ()
    {
        if (_swingTransform == null)
        {
            Debug.LogError("No Swing transform in MeleeAttack!");
            return;
        }

        _swingSprite = _swingTransform.GetComponent<SpriteRenderer>();
        _swingCollider = _swingTransform.GetComponent<Collider2D>();
        _swing = _swingTransform.GetComponent<Swing>();

        _swingCollider.enabled = false;
        _originalColor = _swingSprite.color;
        _swingSprite.color = Color.clear;
        _swing.DamagableHit.AddListener(OnDamagableHit);
    }

    public void Fire (Vector2 direction)
    {
        if (!_isAttacking)
        {
            _swingTransform.position = transform.position + (Vector3) direction * _swingOffset;
            _swingTransform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            StartCoroutine(AttackAnimation());
            if (LeavesAoe)
            {
                SpawnAoeEffect(_swingTransform.position);
            }
        }
    }

    private void OnDestroy ()
    {
        StopAllCoroutines();
        _swing.DamagableHit.RemoveListener(OnDamagableHit);
    }


    private IEnumerator AttackAnimation ()
    {
        _isAttacking = true;
        _swingCollider.enabled = true;
        float timeElapsed = 0f;
        
        while (timeElapsed < _attackTime * 0.5f)
        {
            _swingSprite.color = Color.Lerp(Color.clear, _originalColor, timeElapsed * 2f / _attackTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        while (timeElapsed < _attackTime * 0.5f)
        {
            _swingSprite.color = Color.Lerp(Color.clear, _originalColor, timeElapsed * 2f / _attackTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _swingSprite.color = Color.clear;
        _swingCollider.enabled = false;
        _isAttacking = false;
    }

    private void OnDamagableHit (Damagable damagable)
    {
        damagable.Hit(Damage);
    }

    private void SpawnAoeEffect(Vector2 position)
    {
        if (_activeAoeEffect != null)
        {
            _activeAoeEffect.Deactivate();
            _activeAoeEffect.AoeFinished.RemoveAllListeners();
        }
        AoeEffect newAoe = Instantiate(_aoeEffectTemplate);
        newAoe.transform.position = position;
        _activeAoeEffect = newAoe;
        newAoe.AoeFinished.AddListener(() => _activeAoeEffect = null);
    }
}
