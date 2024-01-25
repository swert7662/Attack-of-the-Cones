using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public partial struct ProjectileSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

        // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
        new LaunchProjectile {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb,
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

[BurstCompile]
public partial struct LaunchProjectile : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;

    // IJobEntity generates a component data query based on the parameters of its `Execute` method.
    // This example queries for all Spawner components and uses `ref` to specify that the operation
    // requires read and write access. Unity processes `Execute` for each entity that matches the
    // component data query.
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref PlayerData player, TargetPosition target)
    {
        float3 NoTarget = float3.zero;

        // If the next spawn time has passed.
        if (player.NextFireTime < ElapsedTime)
        {
            if (math.all(player.AttackTarget == NoTarget)) { return; }
            // Spawns a new entity and positions it at the spawner.
            Entity projectile = Ecb.Instantiate(chunkIndex, player.Projectile);
            
            Ecb.SetComponent(chunkIndex, projectile, new LocalTransform { Position = target.Position });

            float3 moveDir = math.normalize(player.AttackTarget - target.Position);
            float angle = math.degrees(math.atan2(moveDir.y, moveDir.x));
            quaternion rotation = quaternion.RotateZ(math.radians(angle));
            Ecb.SetComponent(chunkIndex, projectile, new LocalTransform { Rotation = rotation });

            // Resets the next spawn time.
            player.NextFireTime = (float)ElapsedTime + player.FireRate;
        }
    }
}