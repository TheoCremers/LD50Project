using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerStats
{
    [Header("Base")]
    public float BaseDamage = 10f;
    public float BaseMoveSpeed = 5f;
    public float BaseMaxHealth = 100f;
    public float BaseHealthRegen = 0f;
    public float BaseArmor = 0f;
    public float BaseMass = 50f;
    public float BaseSummonCooldown = 5f;
    public float BaseAttackCooldown = 1f;

    [Header("Multiplier Factors")]
    public float DamageFactor = 1f;
    public float MoveSpeedFactor = 1f;
    public float MaxHealthFactor = 1f;
    public float HealthRegenFactor = 1f;
    public float ArmorFactor = 1f;
    public float MassFactor = 1f;
    public float AttackCooldownFactor = 1f;
    public float SummonCooldownFactor = 1f;

    [Header("Bonus")]
    public float DamageBonus = 0f;
    public float MoveSpeedBonus = 0f;
    public float MaxHealthBonus = 0f;
    public float HealthRegenBonus = 0f;
    public float ArmorBonus = 0f;
    public float MassBonus = 0f;

    public float Damage { get { return BaseDamage * DamageFactor + DamageBonus; } }
    public float MoveSpeed { get { return BaseMoveSpeed * MoveSpeedFactor + MoveSpeedBonus; } }
    public float MaxHealth { get { return BaseMaxHealth * MaxHealthFactor + MaxHealthBonus; } }
    public float HealthRegen { get { return BaseHealthRegen * HealthRegenFactor + HealthRegenBonus; } }
    public float Armor { get { return BaseArmor * ArmorFactor + ArmorBonus; } }
    public float Mass { get { return BaseMass * MassFactor + MassBonus; } }
    public float AttackCooldown { get { return BaseAttackCooldown * AttackCooldownFactor; } }
    public float SummonCooldown { get { return BaseSummonCooldown * SummonCooldownFactor; } }

}
