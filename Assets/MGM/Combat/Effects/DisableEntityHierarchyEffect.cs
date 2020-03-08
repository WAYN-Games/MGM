using Unity.Entities;
using Wayn.Mgm.Effects;

namespace Wayn.Mgm.Combat.Effects
{

    /// <summary>
    /// This effect disable the Target entity and all it's children.
    /// </summary>
    public struct DisableEntityHierarchyEffect : IEffectApplierWithECB
    {

        /// <summary>
        /// Represents the root of the affected hoerarchy.
        /// </summary>
        public Entity Other { get; set; }
        
        /// <summary>
        /// The entity responsible for triggering the effect.
        /// </summary>
        public Entity Emmiter { get; set; }

        /// <summary>
        /// Define if the effect should apply to children.
        /// </summary>
        public bool ApplyRecursivelyToChildren { get => true; set => ApplyRecursivelyToChildren = true; }

        /// <summary>
        /// Apply the effect to the target (not the Target).
        /// </summary>
        /// <param name="jobIndex">Unique index of the job processing the effect.</param>
        /// <param name="target">Represent the current target of the effect within the hiearchy.</param>
        /// <param name="ecb">The Entty Command Buffer Concurent used to apply structural changes.</param>
        public void Apply(in int jobIndex, Entity target, ref EntityCommandBuffer.Concurrent ecb)
        {
            ecb.AddComponent(jobIndex,target, new Disabled());
        }
    }

    /// <summary>
    /// This system applies the queued up DestroyEntityHirearchyEffect.
    /// </summary>
    /// <summary>
    /// This system applies the queued up DisableEntityHirearchyEffect.
    /// </summary>
    public class DisableEntityHierarchyEffectConsumerSystem : EffectConsumerSystemWithECB<DisableEntityHierarchyEffect>
    {
    }
}