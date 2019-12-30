using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct GroundInfo : IComponentData
{
    public bool IsGrounded;
    public float GroundCheckDistance;
    public float3 GroundNormal;
}
