using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class TargetPositionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 enemyTarget = GameManager.Instance._playerTransform.position;

        EntityQuery playerQuery = GetEntityQuery(typeof(PlayerData));
        if (playerQuery.CalculateEntityCount() == 0)
            return;
        Entity playerEntity = playerQuery.GetSingletonEntity();

        float3 playerTarget = World.EntityManager.GetComponentData<PlayerData>(playerEntity).AttackTarget;   

        foreach (MoveToTargetPositionAspect moveToTargetPositionAspect in SystemAPI.Query<MoveToTargetPositionAspect>().WithAll<EnemyTag>())
        {
            moveToTargetPositionAspect.SetTargetPosition(enemyTarget);
        }

        foreach (MoveToTargetPositionAspect moveToTargetPositionAspect in SystemAPI.Query<MoveToTargetPositionAspect>().WithAll<PlayerData>())
        {
            moveToTargetPositionAspect.SetTargetPosition(enemyTarget);
        }

        foreach (MoveToTargetPositionAspect moveToTargetPositionAspect in SystemAPI.Query<MoveToTargetPositionAspect>().WithAll<ProjectileComponent>())
        {
            if(moveToTargetPositionAspect.TargetSet() == false)
                moveToTargetPositionAspect.SetTargetPosition(playerTarget);
        }
    }
}
