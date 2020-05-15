using System;

namespace Wayn.Mgm.Event.Registry
{
    public struct RegistryEventReference
    {
        public int TypeId;
        public int VersionId;

        public static int GetTypeId(Type t)
        {
            return t.GetHashCode();
        }

        public static int GetEffectInstanceId(IRegistryEvent effect)
        {
            return effect.GetHashCode();
        }

    }
}
