using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Heading : IComponentData
    {

        public float3 Value;
    }
}
