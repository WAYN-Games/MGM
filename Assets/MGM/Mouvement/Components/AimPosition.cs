using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct AimPosition : IComponentData
{
    public float3 Value;    
}
