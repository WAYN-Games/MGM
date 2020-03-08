using Unity.Entities;
using Wayn.Mgm.Effects;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect add the Amount the the Target entity's Health
    /// </summary>
    public struct ChangeHealthEffect : IEffectApplier<Health>
    {
        /// <summary>
        /// The entity affected by the effect.
        /// </summary>
        public Entity Other { get; set; }

        /// <summary>
        /// The entity responsible for triggering the effect.
        /// </summary>
        public Entity Emmiter { get; set; }

        /// <summary>
        /// The amount of health changed.
        /// </summary>
        public float Amount;

        /// <summary>
        /// This method apply the effect to the affected Target.
        /// </summary>
        /// <param name="healths"></param>
        public void Apply(ref ComponentDataFromEntity<Health> healths)
        {
            Health health = healths[Other];
            health.Value += Amount;
            healths[Other] = health;
        }
    }

    /// <summary>
    /// This system applies the queued up ChangeHealthEffect to the Health componentdata.
    /// </summary>
    [UpdateAfter(typeof(DisableEntityHierarchyEffectConsumerSystem))]
    public class ChangeHealthEffectConsumerSystem : EffectConsumerSystem<ChangeHealthEffect, Health>
    {
    }
}
