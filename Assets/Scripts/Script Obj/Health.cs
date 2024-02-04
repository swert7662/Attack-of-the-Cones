using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health", order = 1)]
public class Health : ScriptableObject
{
    public float CurrentHealth;
    public float MaxHealth;
}