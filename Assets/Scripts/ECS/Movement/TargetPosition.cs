using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct TargetPosition : IComponentData
{
    public float3 Position;
    public float3 Direction;
    public bool IsSet;
}
