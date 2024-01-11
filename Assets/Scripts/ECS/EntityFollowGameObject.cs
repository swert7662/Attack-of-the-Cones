using Unity.Entities;
using UnityEngine;

public class EntityFollowGameObject : MonoBehaviour
{
    public Entity entity;
    private EntityManager entityManager;
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        if (entityManager != null && entity != Entity.Null)
        {
            entityManager.SetComponentData(entity, new GameObjectLocation { Position = transform.position });
        }
    }
}
