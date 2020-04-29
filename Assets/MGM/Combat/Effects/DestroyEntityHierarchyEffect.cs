using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect destroy the Target entity and all it's children.
    /// </summary>
    [Serializable]
    public struct DestroyEntityHierarchyEffect : IEffect
    {
        public bool ApplyRecursivelyToChildren;

    }

    /// <summary>
    /// This system applies the queued up DestroyEntityHirearchyEffect.
    /// </summary>
    public class DestroyEntityHirearchyEffectConsumerSystem : EffectConsumerSystem<DestroyEntityHierarchyEffect>
    {
        private BeginSimulationEntityCommandBufferSystem ECBSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            ECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle ScheduleJob(
            JobHandle inputDeps,
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, DestroyEntityHierarchyEffect> RegisteredEffects)
        {
            JobHandle jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                EntityCommandBuffer = ECBSystem.CreateCommandBuffer(),
                Children = GetBufferFromEntity<Child>(true)
            }.Schedule(inputDeps);
            ECBSystem.AddJobHandleForProducer(jh);
            return jh;
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {

            [ReadOnly]
            public NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, DestroyEntityHierarchyEffect> RegisteredEffects;
            [ReadOnly]
            public BufferFromEntity<Child> Children;

            public EntityCommandBuffer EntityCommandBuffer;


            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    DestroyEntityHierarchyEffect effect;
                    if (RegisteredEffects.TryGetValue(command.RegistryReference.VersionId, out effect))
                    {
                        RecursiveChildEffect(command.Target, effect);
                    }
                }
            }

            private void RecursiveChildEffect(Entity target, DestroyEntityHierarchyEffect effect)
            {
                EntityCommandBuffer.DestroyEntity(target);
                if (effect.ApplyRecursivelyToChildren && Children.Exists(target))
                {
                    var enumerator = Children[target].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Entity e = enumerator.Current.Value;
                        RecursiveChildEffect(e, effect);
                    }
                }
            }
        }
    }


}