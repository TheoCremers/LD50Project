using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AoeEffect : MonoBehaviour
{
    public float duration = 3f;
    public float activeFor = 0f;
    public int damagePerTick = 2;
    public float damageInterval = 0.5f;
    public float fadeTime = 0.2f;

    public UnityEvent AoeFinished;

    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Color _baseColor;
    private Color _offColor;

    private void Start ()
    {
        _baseColor = _spriteRenderer.color;
        _offColor = _baseColor;
        _offColor.a = 0f;

        Activate();
    }

    public void Activate ()
    {
        StartCoroutine(FullSequence());
    }

    public void Deactivate ()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FullSequence ()
    {
        yield return FadeIn();
        yield return TriggerDamageAoe();
        yield return FadeOut();
    }

    private IEnumerator TriggerDamageAoe ()
    {
        _spriteRenderer.color = _baseColor;
        float localTime = 0f;

        while (localTime < duration + fadeTime)
        {
            yield return null;
            _collider.enabled = true;
            yield return new WaitForSeconds(damageInterval);
            _collider.enabled = false;
            localTime += damageInterval + Time.deltaTime;
        }

        _spriteRenderer.color = _offColor;
    }

    private IEnumerator FadeIn ()
    {
        gameObject.SetActive(true);
        float localTime = 0f;
        _collider.enabled = false;
        if (localTime < fadeTime)
        {
            _spriteRenderer.color = Color.Lerp(_offColor, _baseColor, localTime / fadeTime);
            localTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(TriggerDamageAoe());
    }

    private IEnumerator FadeOut ()
    {
        float localTime = 0f;
        _collider.enabled = false;
        while (localTime < fadeTime)
        {
            _spriteRenderer.color = Color.Lerp(_baseColor, _offColor, localTime / fadeTime);
            localTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy ()
    {
        AoeFinished?.Invoke();
        AoeFinished.RemoveAllListeners();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.TryGetComponent(out Damagable damagable))
        {
            damagable.Hit(damagePerTick);
        }
    }
}
