using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Health : IComponentData
    {
        public int Value;
    }



    public struct MaxHealth : IComponentData
    {
        public int Value;
    }
}
