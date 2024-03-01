using UnityEngine;

public class Burning : MonoBehaviour
{
    private int Damage;
    private float MaxLifetime; // Duration of the effect
    private float TickRate; // Damage tick rate (twice per second)
    private float TickTimer; // Timer to track when to apply the next tick of damage
    private float Timer; // Timer to track the remaining duration of the effect   

    public void Initialization(int damage, float duration, float tickRate)
    {
        Damage = damage;
        MaxLifetime = duration;
        TickRate = tickRate;

        Timer = MaxLifetime;        
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        TickTimer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(this);
        }

        else if (TickTimer <= 0)
        {
            TickTimer = TickRate; 
            ApplyDamage(); 
        }
    }

    private void ApplyDamage()
    {
        IHealth enemyHealth = GetComponent<IHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.Damage(Damage, DamageType.Fire);
        }
    }


    public void ResetTimer()
    {
        Timer = MaxLifetime;
    }
    private void OnDisable()
    {
        Destroy(this);
    }

}
