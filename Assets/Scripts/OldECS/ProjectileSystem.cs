using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct ProjectileSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new MoveProjectile {
            DeltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct MoveProjectile : IJobEntity
    {
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, ref Speed speed, ProjectileComponent projectile)
        {
            float angle = math.atan2(transform.Rotation.value.y, transform.Rotation.value.x) * 2.0f;

            float2 direction = new float2(math.cos(angle), math.sin(angle));

            transform.Position += new float3(direction.x, direction.y, 0) * speed.value * DeltaTime;

        }
    }

}
