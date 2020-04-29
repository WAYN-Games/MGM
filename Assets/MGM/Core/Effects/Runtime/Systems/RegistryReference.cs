using System;
using Unity.Entities;

namespace Wayn.Mgm.Events.Registry
{
    public struct RegistryReference
    {
        public int TypeId;
        public int VersionId;

        public static int GetTypeId(Type t)
        {
            return t.GetHashCode();
        }

        public static int GetEffectInstanceId(IRegistryElement effect)
        {
            return effect.GetHashCode();
        }

    }
}
