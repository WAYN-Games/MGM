using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct JumpCount : IComponentData
{
    public int Value;
}
