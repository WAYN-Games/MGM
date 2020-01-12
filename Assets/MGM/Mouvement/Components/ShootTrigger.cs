using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct ShootTrigger : IComponentData
{
    public bool Value;
}