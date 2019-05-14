using System;
using Unity.Entities;
using Unity.Mathematics;


namespace MGM {
    [Serializable]
    public struct Health : IComponentData
    {
        public int Value;
    }


    public struct Update<T>
    {

        

        public static T operator +(T x, T y)
        {
            
            return x.Value + y.Value);
        }
    }
    public struct MaxHealth : IComponentData
    {
        public int Value;
    }
}
