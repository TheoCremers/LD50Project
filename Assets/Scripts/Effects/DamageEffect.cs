using System.Collections;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private float _flashFrameDuration = 0.05f;
    private ParticleSystem _particleSystem = null;
    private Color _originalColor;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _originalColor = _spriteRenderer.color;
    }

    public void TriggerDamageEffect()
    {
        _particleSystem.Emit(1);
        StopCoroutine(DamageFlash());
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash ()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(_flashFrameDuration);
        _spriteRenderer.color = _originalColor;
    }
}
