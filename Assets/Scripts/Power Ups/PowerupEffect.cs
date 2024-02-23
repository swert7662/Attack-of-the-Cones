using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    public Sprite Image;

    public abstract void Apply();
}
