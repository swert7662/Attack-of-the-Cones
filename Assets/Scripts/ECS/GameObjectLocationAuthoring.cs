using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

class GameObjectLocationAuthoring : MonoBehaviour
{
    public float3 Position;
}

class GameObjectLocationBaker : Baker<GameObjectLocationAuthoring>
{
    public override void Bake(GameObjectLocationAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new GameObjectLocation { Position = authoring.Position });

        Debug.Log("Component added with position: " + authoring.Position);
    }
}