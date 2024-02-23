using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "ScriptableObjects/Powerup", order = 4)]
public class Powerup : ScriptableObject
{
    public Collectible.PowerUpType Type;
    public Sprite Image;
}
