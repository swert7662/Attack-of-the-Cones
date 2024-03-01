using System;
using UnityEngine;

public interface IHealth
{
    void Damage(float damageAmount, DamageType damageType);
    void Die();
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    Vector3 Extents { get; set; }
}
