using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class TargetPositionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 currentTarget = GameManager.Instance._playerTransform.position;

        foreach (MoveToTargetPositionAspect moveToTargetPositionAspect in SystemAPI.Query<MoveToTargetPositionAspect>())
        {
            moveToTargetPositionAspect.SetTargetPosition(currentTarget);
        }
    }
}
