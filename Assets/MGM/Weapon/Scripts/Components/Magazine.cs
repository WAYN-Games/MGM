using System;
using Unity.Entities;

namespace MGM.Weapon
{

    [Serializable]
    public struct Magazine : IComponentData
    {

        public int MaxCapacity;
        public int CurrentCapacity;

        public bool IsMagazineEmpty(ref Magazine magazine)
        {
            // Allow for infinite magazine capacity
            if (magazine.CurrentCapacity == -1) return false;

            // Shoot only if projectile left.
            if (magazine.CurrentCapacity == 0) return true;
            // remove one projectile from the magazine.
            magazine.CurrentCapacity -= 1;
            return false;
        }
    }
}
