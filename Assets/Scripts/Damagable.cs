using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 20;
    [SerializeField]
    private int _health;

    public int Health 
    {
        get { return _health; }
        set 
        {
            _health = value;
            OnHealthChange?.Invoke((float)Health / _maxHealth);
        }
    }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChange;
    public UnityEvent OnHit, OnHeal;

    private void Start()
    {
        Health = _maxHealth;
    }

    internal void Hit(int damage) 
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

    public void Heal(int healPoints) 
    {
        Health += healPoints;
        Health = Mathf.Clamp(Health, 0, _maxHealth);
        OnHeal?.Invoke();
    }
}
