using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct ProjectileEntityReference : IComponentData
{
    public Entity Value;
}
