using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct MoveToTargetPositionAspect : IAspect
{
    private readonly Entity entity;

    private readonly RefRO<Speed> speed;

    private readonly RefRW<LocalTransform> transform;
    private readonly RefRW<TargetPosition> targetPosition;

    public void Move(float deltaTime, bool follow)
    {
        if (follow)
        {
            SetTargetDirection();
        }           
        transform.ValueRW.Position += targetPosition.ValueRW.Direction * speed.ValueRO.value * deltaTime;
    }

    public void SetTargetDirection()
    {
        if (math.distance(transform.ValueRW.Position, targetPosition.ValueRW.Position) < .1f)
        {
            return;
        }
        targetPosition.ValueRW.Direction = math.normalize(targetPosition.ValueRW.Position - transform.ValueRW.Position);
        targetPosition.ValueRW.IsSet = true;
    }

    public void SetTargetDirection(float3 position)
    {
        if (math.distance(transform.ValueRW.Position, position) < .1f)
        {
            return;
        }
        targetPosition.ValueRW.Direction = math.normalize(position - transform.ValueRW.Position);
        targetPosition.ValueRW.IsSet = true;
    }

    public void SetTargetPosition(float3 position)
    {
        targetPosition.ValueRW.Position = position;
        SetTargetDirection(position);        
    }

    public bool TargetSet()
    {
        return targetPosition.ValueRW.IsSet;
    }
}
