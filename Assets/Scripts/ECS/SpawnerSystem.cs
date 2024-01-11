using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public partial struct OptimizedSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

        // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
        new ProcessSpawnerJob {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb,
            spawnPositionOffset = GenerateRandomOffCameraPosition()
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }

    private float3 GenerateRandomOffCameraPosition()
    {
        // Randomly choose whether to place the X or Y coordinate off-camera
        bool isXOffCamera = UnityEngine.Random.value > 0.5f;

        float x, y;

        if (isXOffCamera)
        {
            // Set X to be off-camera (either less than -25 or greater than 25)
            x = (UnityEngine.Random.value > 0.5f ? 1 : -1) * (25 + UnityEngine.Random.value * 25);
            // Y can be anywhere between -25 and 25
            y = UnityEngine.Random.Range(-25f, 25f);
        }
        else
        {
            // Set Y to be off-camera
            y = (UnityEngine.Random.value > 0.5f ? 1 : -1) * (25 + UnityEngine.Random.value * 25);
            // X can be anywhere between -25 and 25
            x = UnityEngine.Random.Range(-25f, 25f);
        }

        // Assuming Z is not relevant for your requirement, or you can set it to a default value
        float z = 0;

        return new float3(x, y, z);
    }
}

[BurstCompile]
public partial struct ProcessSpawnerJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;
    public float3 spawnPositionOffset;

    // IJobEntity generates a component data query based on the parameters of its `Execute` method.
    // This example queries for all Spawner components and uses `ref` to specify that the operation
    // requires read and write access. Unity processes `Execute` for each entity that matches the
    // component data query.
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner, TargetPosition targetPosition)
    {
        // If the next spawn time has passed.
        if (spawner.NextSpawnTime < ElapsedTime)
        {
            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);

            Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(targetPosition.Position + spawnPositionOffset));

            // Resets the next spawn time.
            spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
        }
    }
}