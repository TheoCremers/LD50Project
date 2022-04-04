using System.Collections;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    private ParticleSystem _particleSystem = null;
    private Color _originalColor;
    [SerializeField] private float _flashFrameDuration = 0.05f;

    // Start is called before the first frame update
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
