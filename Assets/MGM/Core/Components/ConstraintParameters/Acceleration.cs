using System;
using Unity.Entities;
using Unity.Mathematics;

namespace MGM
{
    [Serializable]
    public struct Acceleration : IComponentData
    {
        public float Linear;
        public float Angular;
    }
}