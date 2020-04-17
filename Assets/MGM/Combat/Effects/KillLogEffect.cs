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
    public struct KillLogEffect : IEffect
    {
        [HideInInspector] // Empty IEffect struct are not supported.
        public bool dummy;
    }

    public class KillLogEffectConsumer : EffectConsumerSystem<KillLogEffect>
    {
        protected override JobHandle ScheduleJob(
            JobHandle inputDeps,
            in NativeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, KillLogEffect> RegisteredEffects)
        {
            return new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator
            }.Schedule(inputDeps);
        }

        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public NativeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator;

            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    Debug.Log($"{EffectCommandEnumerator.Current.Emitter} killed {EffectCommandEnumerator.Current.Target}.");
                }
            }
        }
    }
}