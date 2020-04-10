using System;
using Unity.Entities;

namespace Wayn.Mgm.Events.Registry
{
    public struct RegistryReference
    {
        public ulong TypeId;
        public int VersionId;

        public static ulong GetTypeId(Type t)
        {
            return TypeHash.CalculateStableTypeHash(t);
        }

        public static int GetEffectInstanceId(IRegistryElement effect)
        {
            return effect.GetHashCode();
        }

    }
}
