using System;
using Unity.Entities;
using Unity.Physics;

[Serializable]
[InternalBufferCapacity(5)]
public struct ContactInfos : IBufferElementData
{
    public Entity Entity;
    public ColliderCastHit Contact;
}
