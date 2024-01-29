using System;
using UnityEngine;

public interface IHealth
{
    event Action<GameObject, Vector2, float> OnDamageTaken;
    // Other health-related methods and properties

    void Damage(float damageAmount);
    void Die();

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    Vector2 Extents { get; set; }
}
