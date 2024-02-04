using UnityEngine;

[System.Serializable]
public class DamagedData : UnityEngine.Object
{
    public GameObject GameObjectSender;
    public float MaxHealth;
    public Vector3 Extents;

    public Vector3 Position;    
    public float DamageAmount;
    public float CurrentHealth;
    

    public DamagedData(GameObject gameObjectSender = null,
                       float maxHealth = 0,
                       Vector3 extents = default(Vector3),
                       Vector3 position = default(Vector3),
                       float damageAmount = 0,
                       float currentHealth = 0)
    {
        GameObjectSender = gameObjectSender;
        MaxHealth = maxHealth;
        Extents = extents;
        Position = position;
        DamageAmount = damageAmount;
        CurrentHealth = currentHealth;
    }
}