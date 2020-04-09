using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Effects;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect destroy the Target entity and all it's children.
    /// </summary>
    public struct DestroyEntityHierarchyEffect : IEffect
    {
        public bool ApplyRecursivelyToChildren;

    }

    /// <summary>
    /// This system applies the queued up DestroyEntityHirearchyEffect.
    /// </summary>
    public class DestroyEntityHirearchyEffectConsumerSystem : EffectConsumerSystem<DestroyEntityHierarchyEffect>
    {

        protected override JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in ulong EffectTypeId,
            in NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap,
            in NativeHashMap<int, DestroyEntityHierarchyEffect> RegisteredEffects,
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
            public NativeHashMap<int, DestroyEntityHierarchyEffect> RegisteredEffects;

            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute()
            {
                foreach (EffectCommand command in EffectCommandMap.GetValuesForKey(EffectTypeId))
                {
                    DestroyEntityHierarchyEffect effect;
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