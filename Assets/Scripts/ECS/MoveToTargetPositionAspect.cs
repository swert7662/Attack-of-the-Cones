using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct MoveToTargetPositionAspect : IAspect
{
    private readonly Entity entity;

    private readonly RefRO<Speed> speed;

    private readonly RefRW<LocalTransform> transform;
    private readonly RefRW<TargetPosition> targetPosition;

    public void Move(float deltaTime)
    {

        if (math.distance(transform.ValueRW.Position, targetPosition.ValueRW.Position) < 5f)
        {
            return;
        }
        float3 direction = math.normalize(targetPosition.ValueRW.Position - transform.ValueRW.Position);
        transform.ValueRW.Position += direction * speed.ValueRO.value * deltaTime;
    }

    public void SetTargetPosition(float3 position)
    {
        targetPosition.ValueRW.Position = position;
    }
}
