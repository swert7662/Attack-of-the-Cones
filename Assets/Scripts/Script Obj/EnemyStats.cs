using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 3)]
public class EnemyStats : ScriptableObject
{
    public LayerMask enemyLayerMask;

    public int baseAttackDamage;
    public int baseMaxHealth;
    public int baseExpPoints;
    public float baseSpeed;
    //public float baseAttackRange;

    public int attackDamage;    
    public int maxHealth;
    public int expPoints;
    public float speed;
    //public float attackRange;

    public void SetStats()
    {
        speed = baseSpeed;
        attackDamage = baseAttackDamage;
        maxHealth = baseMaxHealth;
        expPoints = baseExpPoints;
    }

    public void UpdateStats()
    {
        // Example scaling: Increase stats by 1% every minute
        float scale = 1.05f;

        speed *= scale;
        attackDamage += 5;
        maxHealth += 10;
    }
}