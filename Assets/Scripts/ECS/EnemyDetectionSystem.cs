using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using System.Diagnostics;

public partial struct EnemyDetectionSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (var (playerData, transform) in SystemAPI.Query<RefRW<PlayerData>, LocalTransform>())
        {
            ClosestHitCollector<DistanceHit> closestHitCollector = new ClosestHitCollector<DistanceHit>(playerData.ValueRO.Range);
            if (physicsWorld.OverlapSphereCustom(transform.Position, playerData.ValueRO.Range, ref closestHitCollector, playerData.ValueRO.ProjectileCollisionFilter))
            {
                //Debug.WriteLine("Enemy detected!" + closestHitCollector.ClosestHit.Entity.ToString());
                playerData.ValueRW.AttackTarget = closestHitCollector.ClosestHit.Position;
            }
            else
            {
                playerData.ValueRW.AttackTarget = float3.zero;
            }
        }        
    }
}
