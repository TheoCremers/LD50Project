using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _swingOffset = 1.0f;
    [SerializeField] private Transform _swingTransform = null;
    [SerializeField] private AoeEffect _aoeEffectTemplate = null;
    private Swing _swing = null;
    private AoeEffect _activeAoeEffect = null;

    public int Damage = 5;
    public bool LeavesAoe = false;

    public SFXType SFX = SFXType.None;

    private void Start ()
    {
        if (_swingTransform == null)
        {
            Debug.LogError("No Swing transform in MeleeAttack!");
            return;
        }

        _swing = _swingTransform.GetComponent<Swing>();

        _swing.DamagableHit.AddListener(OnDamagableHit);
    }

    public void Fire (Vector2 direction)
    {
        _swingTransform.position = transform.position + (Vector3) direction * _swingOffset;
        _swingTransform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        _swing.Activate();

        if (LeavesAoe)
        {
            SpawnAoeEffect(_swingTransform.position);
        }

        AudioManager.PlaySFXVariation(SFX, gameObject);
    }

    private void OnDestroy ()
    {
        StopAllCoroutines();
        _swing.DamagableHit.RemoveListener(OnDamagableHit);
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
