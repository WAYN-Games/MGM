using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Effects;

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
        protected override JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in ulong EffectTypeId,
            in NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap,
            in NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects,
            ref EntityCommandBuffer EntityCommandBuffer)
        {
            return new ConsumerJob()
            {
                EffectCommandMap = EffectCommandMap,
                EffectTypeId = EffectTypeId,
                EntityCommandBuffer = EntityCommandBuffer,
                RegisteredEffects = RegisteredEffects
            }.Schedule(inputDeps);
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            
            [ReadOnly]
            public NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap;
            [ReadOnly]
            public ulong EffectTypeId;
            [ReadOnly]
            public NativeHashMap<int, DisableEntityHierarchyEffect> RegisteredEffects;

            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute()
            {
                foreach (EffectCommand command in EffectCommandMap.GetValuesForKey(EffectTypeId))
                {
                    DisableEntityHierarchyEffect effect;
                   // Debug.Log($"DisableEntityHierarchyEffectConsumer applying effect {command.Emitter}/{command.Target}/{command.EffectReference.TypeId}/{command.EffectReference.VersionId}");
                    if (RegisteredEffects.TryGetValue(command.EffectReference.VersionId, out effect))
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