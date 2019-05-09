using System;
using Unity.Entities;
using Unity.Mathematics;

namespace MGM
{
    [Serializable]
    public struct MaxAge : IComponentData
    {
        public float Value;
    }
}