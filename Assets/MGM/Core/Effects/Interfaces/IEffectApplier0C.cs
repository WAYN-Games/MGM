using Unity.Entities;
namespace Wayn.Mgm.Effects
{
    public interface IEffectApplierWithECB : IEffect
    {
        bool ApplyRecursivelyToChildren { get; set; }

        void Apply(in int jobIndex, Entity target, ref EntityCommandBuffer.Concurrent ecb);
    }

    public interface IEffectApplierWithDeltaTimeAndECB : IEffect
    {
        bool ApplyRecursivelyToChildren { get; set; }

        void Apply(in float deltaTime, in int jobIndex, Entity target, ref EntityCommandBuffer.Concurrent ecb);
    }
}
