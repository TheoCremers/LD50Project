using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField]
    public float MaxHealth = 20f;
    [SerializeField]
    private float _health;

    public float BaseRegen = 0f;
    public float RegenFactor = 1f;
    public float RegenTimer = 1f;

    public float Health 
    {
        get { return _health; }
        set 
        {
            _health = value;
            OnHealthChange?.Invoke((float)Health / MaxHealth);
        }
    }

    public float HealthPercentage
    {
        get { return (Health / MaxHealth); }
    }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChange;
    public UnityEvent OnHit, OnHeal;

    private void Start()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        if (BaseRegen > 0f) 
        {
            if (RegenTimer <= 0f) 
            {
                Heal((MaxHealth / 100f) * BaseRegen * RegenFactor);
                RegenTimer = 1f;
            } 
            else
            {
                RegenTimer -= Time.deltaTime;
            }
        }
    }

    internal void Hit(float damage) 
    {
        Health -= damage;
        if (Health <= 0) 
        {
            OnDeath?.Invoke();
        } 
        else 
        {
            OnHit?.Invoke();
        }
    }

    public void Heal(float healPoints) 
    {
        Health += healPoints;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnHeal?.Invoke();
    }
}
