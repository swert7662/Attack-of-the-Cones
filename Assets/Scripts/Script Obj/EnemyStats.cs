using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 3)]
public class EnemyStats : ScriptableObject
{
    public LayerMask enemyLayerMask;

    public int attackDamage;    
    public int maxHealth;
    public int expPoints;
    public float speed;
    //public float attackRange;

    public void UpdateStats(EnemyStats newStats)
    {
        speed = newStats.speed;
        attackDamage = newStats.attackDamage;
        maxHealth = newStats.maxHealth;
        expPoints = newStats.expPoints;
    }
}