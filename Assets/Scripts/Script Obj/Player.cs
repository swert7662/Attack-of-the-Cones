using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 2)]
public class Player : ScriptableObject
{
    public Vector3 Position;
    public bool IsAlive;

    public Component Collider { get; private set; }

    public void SetCollider(Component collider)
    {
        Collider = collider;
    }
}