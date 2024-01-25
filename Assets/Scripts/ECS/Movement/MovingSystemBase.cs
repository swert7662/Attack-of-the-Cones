using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct MovingSystemBase : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new MoveJob {
            DeltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }
    
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(MoveToTargetPositionAspect moveToTargetPositionAspect)
    {
        moveToTargetPositionAspect.Move(DeltaTime, true);
    }
}
