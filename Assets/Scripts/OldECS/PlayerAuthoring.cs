using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public GameObject Projectile;
    public float FireRate;
    public float Range;
    public float3 AttackTarget;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var filter = CollisionFilter.Default;
            filter.CollidesWith = authoring.Projectile.GetComponent<PhysicsShapeAuthoring>().CollidesWith.Value;
            filter.BelongsTo = authoring.Projectile.GetComponent<PhysicsShapeAuthoring>().BelongsTo.Value;

            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerData {
                Projectile = GetEntity(authoring.Projectile, TransformUsageFlags.Dynamic),
                FireRate = authoring.FireRate,
                NextFireTime = 0.0f,
                Range = authoring.Range,
                AttackTarget = 0,
                ProjectileCollisionFilter = filter
            });
        }
    }
}
