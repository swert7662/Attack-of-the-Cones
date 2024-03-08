using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 2)]
public class PlayerStats : Stats
{
    [SerializeField] private PlayerStats _baseStats;

    public Vector3 Position;
    public bool IsAlive;

    // -------------- Player Current Stats --------------
    public int Damage;
    public float FireRate;
    public float Health;
    public float Speed;
    public float SuctionRange;
    public short BonusXP;

    public void ResetStats()
    {
        Damage = _baseStats.Damage;
        FireRate = _baseStats.FireRate;            
        Health = _baseStats.Health;
        Speed = _baseStats.Speed;
        SuctionRange = _baseStats.SuctionRange;
        BonusXP = _baseStats.BonusXP;
    }

    public Projectile Projectile { get; private set; }

    public void SetProjectile(Projectile projectile)
    {
        Projectile = projectile;
    }
    public Transform Transform { get; private set; }

    public void SetTransform (Transform transform)
    {
        Transform = transform;
    }

    public Component Collider { get; private set; }

    public void SetCollider(Component collider)
    {
        Collider = collider;
    }

    public Transform LastFollower { get; private set; }

    public void SetLastFollower(Transform follower)
    {
        LastFollower = follower;
    }
}