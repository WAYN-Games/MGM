using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Damage : IComponentData
    {
        public int Value;
    }
}
