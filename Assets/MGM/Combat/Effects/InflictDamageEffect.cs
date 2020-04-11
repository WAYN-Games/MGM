using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
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
            in UnsafeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, InflictDamageEffect> RegisteredEffects)
        {
            return new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                Healths = GetComponentDataFromEntity<Health>(false)
            }.Schedule(inputDeps);
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public UnsafeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, InflictDamageEffect> RegisteredEffects;

            public ComponentDataFromEntity<Health> Healths;

            public void Execute()
            {
                while(EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    if (!Healths.Exists(command.Target)) continue;
                   
                    Health health = Healths[command.Target];
                    health.Value -= RegisteredEffects[command.RegistryReference.VersionId].Amount;
                    Healths[command.Target] = health;



                }
            }
        }
    }

}