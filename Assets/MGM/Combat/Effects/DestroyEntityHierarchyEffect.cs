using Unity.Entities;
using Wayn.Mgm.Effects;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect destroy the Target entity and all it's children.
    /// </summary>
    public struct DestroyEntityHierarchyEffect : IEffectApplierWithECB
    {
        public Entity Other { get; set; }
        
        /// <summary>
        /// The entity responsible for triggering the effect.
        /// </summary>
        public Entity Emmiter { get; set; }

        public bool ApplyRecursivelyToChildren { get => true; set => ApplyRecursivelyToChildren = true; }

        public void Apply(in int jobIndex, Entity target, ref EntityCommandBuffer.Concurrent ecb)
        {
            ecb.DestroyEntity(jobIndex, target);
        }

    }

    /// <summary>
    /// This system applies the queued up DestroyEntityHirearchyEffect.
    /// </summary>
    public class DestroyEntityHirearchyEffectConsumerSystem : EffectConsumerSystemWithECB<DestroyEntityHierarchyEffect>
    {
    }
}