using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Target : IComponentData
    {
        public Entity Entity;
    }

    public struct IsTargeted : IComponentData
    {
    }
}
