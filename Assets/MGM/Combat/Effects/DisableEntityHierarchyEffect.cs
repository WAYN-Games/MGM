using System;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

using Wayn.Mgm.Event;
using Wayn.Mgm.Event.Registry;

namespace Wayn.Mgm.Combat.Effects
{

    /// <summary>
    /// This effect disable the Target entity and all it's children.
    /// </summary>
    [Serializable]
    public struct DisableEntityHierarchyEffect : IEffect
    {
        public bool ApplyRecursivelyToChildren;
    }

    /// <summary>
    /// This system applies the queued up DestroyEntityHirearchyEffect.
    /// </summary>
    /// <summary>
    /// This system applies the queued up DisableEntityHirearchyEffect.
    /// </summary>
    public class DisableEntityHierarchyEffectConsumerSystem : EffectConsumerSystem<DisableEntityHierarchyEffect>
    {
        private BeginSimulationEntityCommandBufferSystem ECBSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            ECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects)
        {
            JobHandle jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                EntityCommandBuffer = ECBSystem.CreateCommandBuffer(),
                Children = GetBufferFromEntity<Child>(true)
            }.Schedule(Dependency);
            ECBSystem.AddJobHandleForProducer(jh);
            return jh;
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {

            [ReadOnly]
            public NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects;
            [ReadOnly]
            public BufferFromEntity<Child> Children;

            public EntityCommandBuffer EntityCommandBuffer;


            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    if (RegisteredEffects.TryGetValue(command.RegistryReference.VersionId, out DisableEntityHierarchyEffect effect))
                    {
                        RecursiveChildEffect(command.Target, effect.ApplyRecursivelyToChildren);
                    }
                }
            }

            private void RecursiveChildEffect(Entity target, bool applyRecursivelyToChildren)
            {

                EntityCommandBuffer.AddComponent(target, new Disabled());
                if (applyRecursivelyToChildren && Children.Exists(target))
                {
                    NativeArray<Child>.Enumerator enumerator = Children[target].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Entity e = enumerator.Current.Value;
                        RecursiveChildEffect(e, applyRecursivelyToChildren);
                    }
                }
            }
        }


    }
}
