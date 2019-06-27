using System;
using Unity.Entities;

namespace MGM.Weapon
{
    [Serializable]
    [WriteGroup(typeof(Shot))]
    public struct GridShot : IComponentData
    {
        public int SizeX;
        public int SizeY;
        public float Density;
    }
}

