using System;
using Unity.Entities;
using Unity.Transforms;

namespace MGM.Common
{

    [Serializable]
    public struct ECS_VFX : IComponentData
    {
        public int vfxIndex;
        public LocalToWorld ltw;
        public bool play;

    }
}
