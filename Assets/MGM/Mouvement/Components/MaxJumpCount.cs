using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct MaxJumpCount : IComponentData
{
    public int Value;    
}
