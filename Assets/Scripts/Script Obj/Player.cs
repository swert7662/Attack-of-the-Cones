using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 2)]
public class Player : ScriptableObject
{
    public Vector3 Position;
    public bool IsAlive;

    // -------------- Player Base Stats --------------
    public int BaseDamage = 10;
    public float BaseFireRate = 1.0f;
    public float BaseHealth = 100.0f;
    public float BaseSpeed = 5.0f;
    public float BaseSuctionRange = 5.0f;
    public short BaseBonusXP = 0;

    // -------------- Player Current Stats --------------
    public int Damage;
    public float FireRate;
    public float Health;
    public float Speed;
    public float SuctionRange;
    public short BonusXP;

    public void SetStats()
    {
        Damage = BaseDamage;
        FireRate = BaseFireRate;
        Health = BaseHealth;
        Speed = BaseSpeed;
        SuctionRange = BaseSuctionRange;
        BonusXP = BaseBonusXP;
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