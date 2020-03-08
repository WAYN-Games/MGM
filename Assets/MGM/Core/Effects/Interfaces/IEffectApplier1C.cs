using Unity.Entities;
namespace Wayn.Mgm.Effects
{
    public interface IEffectApplier<C1> : IEffect
    where C1 : struct, IComponentData
    {
        void Apply(ref ComponentDataFromEntity<C1> component1);
    }

    public interface IEffectApplierWithECB<C1> : IEffect
        where C1 : struct, IComponentData
    {
        bool ApplyRecursivelyToChildren { get; set; }

        void Apply(in int jobIndex, ref EntityCommandBuffer.Concurrent ecb, Entity target, ref ComponentDataFromEntity<C1> component1);
    }

    public interface IEffectApplierWithDeltaTime<C1> : IEffect
        where C1 : struct, IComponentData
    {
        void Apply(in float deltaTime, ref ComponentDataFromEntity<C1> component1);
    }


    public interface IEffectApplierWithDeltaTimeAndECB<C1> : IEffect
        where C1 : struct, IComponentData
    {
        bool ApplyRecursivelyToChildren { get; set; }

        void Apply(in float deltaTime, in int jobIndex, ref EntityCommandBuffer.Concurrent ecb, Entity target, ref ComponentDataFromEntity<C1> component1);
    }
}