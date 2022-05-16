using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public float MaxHealth = 20f;
    [SerializeField]
    private float _health;

    public float MaxHpMultiplier = 1f;

    public float BaseRegen = 0f;
    public float RegenFactor = 1.5f;
    public float RegenTimer = 1f;

    public float Health 
    {
        get { return _health; }
        set 
        {
            _health = value;
            OnHealthChange?.Invoke((float)Health / (MaxHealth * MaxHpMultiplier));
        }
    }

    public float HealthPercentage
    {
        get { return (Health / (MaxHealth * MaxHpMultiplier)); }
    }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChange;
    public UnityEvent OnHit, OnHeal;

    private void Start()
    {
        Health = MaxHealth * MaxHpMultiplier;
    }

    private void Update()
    {
        if (BaseRegen > 0f) 
        {
            if (RegenTimer <= 0f) 
            {
                Heal(((MaxHealth * MaxHpMultiplier) / 100f) * BaseRegen * RegenFactor);
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
        Health = Mathf.Clamp(Health, 0, (MaxHealth * MaxHpMultiplier));
        OnHeal?.Invoke();
    }

    public void ChangeMaxHealthAndScaleCurrent(int value)
    {
        float currentPercentage = HealthPercentage;
        MaxHealth += value;
        Health = currentPercentage * MaxHealth * MaxHpMultiplier;
    }
}
