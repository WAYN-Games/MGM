using System;
using Unity.Entities;

namespace Wayn.Mgm.Effects
{
    public struct EffectReference
    {
        public ulong TypeId;
        public int VersionId;

        public static ulong GetTypeId(Type t)
        {
            return TypeHash.CalculateStableTypeHash(t);
        }

        public static int GetEffectInstanceId(IEffect effect)
        {
            return effect.GetHashCode();
        }

    }
}
