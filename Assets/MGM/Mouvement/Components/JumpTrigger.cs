using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct JumpTrigger : IComponentData
{
    [HideInInspector]
    public bool Value;
}
