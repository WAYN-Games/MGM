using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct CurrentAge : IComponentData
    {

        public float Value;
    }
}
