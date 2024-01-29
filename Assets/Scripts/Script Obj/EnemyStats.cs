using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public float baseSpeed;
    public float baseAttackDamage;
    public float baseAttackRange;
    public float baseMaxHealth;
    public float baseExpPoints;

    public float speed;
    public float attackDamage;
    public float attackRange;
    public float maxHealth;
    public float expPoints;

    public void SetStats()
    {
        speed = baseSpeed;
        attackDamage = baseAttackDamage;
        attackRange = baseAttackRange;
        maxHealth = baseMaxHealth;
        expPoints = baseExpPoints;
    }

    public void UpdateStats()
    {
        // Example scaling: Increase stats by 1% every minute
        float scale = 1.05f;

        speed *= scale;
        attackDamage *= scale;
        attackRange *= scale;
        maxHealth *= scale;
        expPoints *= scale;
    }
}