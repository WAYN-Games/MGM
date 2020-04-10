using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Events;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect add the Amount the the Target entity's Health
    /// </summary>
    [Serializable]
    public struct InflictDamageEffect : IEffect
    {
        /// <summary>
        /// The amount of health changed.
        /// </summary>
        public float Amount;
    }

    public class InflictDamageEffectConsumer : EffectConsumerSystem<InflictDamageEffect>
    {

        protected override JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in ulong EffectTypeId,
            in NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap,
            in NativeHashMap<int, InflictDamageEffect> RegisteredEffects,
            ref EntityCommandBuffer EntityCommandBuffer)
        {
            return new ConsumerJob()
            {
                EffectCommandMap = EffectCommandMap,
                EffectTypeId = EffectTypeId,
                EntityCommandBuffer = EntityCommandBuffer,
                RegisteredEffects = RegisteredEffects,
                Healths = GetComponentDataFromEntity<Health>(false)
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
            public NativeHashMap<int, InflictDamageEffect> RegisteredEffects;

            public EntityCommandBuffer EntityCommandBuffer;

            public ComponentDataFromEntity<Health> Healths;

            public void Execute()
            {
                foreach (EffectCommand command in EffectCommandMap.GetValuesForKey(EffectTypeId))
                {
                    InflictDamageEffect effect;
                   
                    if (RegisteredEffects.TryGetValue(command.RegistryReference.VersionId, out effect))
                    {

                        if (!Healths.Exists(command.Target)) continue;

                        Health health = Healths[command.Target];
                        health.Value -= effect.Amount;
                        Healths[command.Target] = health;
                    }
                }
            }
        }
    }

}