using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Events;

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
        private EndSimulationEntityCommandBufferSystem ECBSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            ECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in UnsafeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects)
        {
            JobHandle jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                EntityCommandBuffer = ECBSystem.CreateCommandBuffer()
            }.Schedule(inputDeps);
            ECBSystem.AddJobHandleForProducer(jh);
            return jh;
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            
            [ReadOnly]
            public UnsafeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects;

            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    DisableEntityHierarchyEffect effect;
                   // Debug.Log($"DisableEntityHierarchyEffectConsumer applying effect {command.Emitter}/{command.Target}/{command.EffectReference.TypeId}/{command.EffectReference.VersionId}");
                    if (RegisteredEffects.TryGetValue(command.RegistryReference.VersionId, out effect))
                    {
                        if (effect.ApplyRecursivelyToChildren)
                        {

                        }
                        EntityCommandBuffer.AddComponent(command.Target, new Disabled());
                    }
                }
            }
        }

       
    }
}