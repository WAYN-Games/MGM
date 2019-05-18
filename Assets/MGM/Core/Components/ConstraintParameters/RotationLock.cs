using Unity.Entities;
using Unity.Mathematics;

namespace MGM
{
    public struct RotationLock : IComponentData
    {
        public bool3 AxisLocks;
    }
}