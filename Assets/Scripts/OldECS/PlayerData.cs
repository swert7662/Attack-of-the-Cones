using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct PlayerData : IComponentData
{
    public Entity Projectile;
    public float FireRate;
    public float NextFireTime;
    public float Range;
    public float3 AttackTarget;
    public CollisionFilter ProjectileCollisionFilter;
}

public readonly partial struct PlayerDataAspect : IAspect
{
    private readonly RefRW<PlayerData> playerData;

    public void SetAttackTarget(float3 position)
    {
        playerData.ValueRW.AttackTarget = position;
    }
}