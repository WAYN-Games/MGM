using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Speed : IComponentData
    {

        public float Value;
    }
}
