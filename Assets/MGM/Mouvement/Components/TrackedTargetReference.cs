using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct TrackedTargetReference : IComponentData
{   
    public Entity Value;
}
