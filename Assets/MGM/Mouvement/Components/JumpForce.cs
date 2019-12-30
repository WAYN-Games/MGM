using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct JumpForce : IComponentData
{
    public float Value;
    [Tooltip("Does JumpForce ignore exising vertical momentum ?")]
    public bool IsAbsolute;
}
