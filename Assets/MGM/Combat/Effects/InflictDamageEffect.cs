using System;
using Unity.Burst;
using Unity.Collections;
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
        private EffectBufferSystem m_EffectCommandSystem;
        private NativeQueue<EffectCommand>.ParallelWriter m_EffectCommandQueue;

        protected override JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in NativeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, InflictDamageEffect> RegisteredEffects)
        {
            var jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                Healths = GetComponentDataFromEntity<Health>(false),
                OnDeathBuffer = GetBufferFromEntity<OnDeath>(true),
                EffectCommandQueue = m_EffectCommandQueue
            }.Schedule(inputDeps);

            m_EffectCommandSystem.AddJobHandleForConsumer(jh);
            return jh;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectCommandSystem = World.GetOrCreateSystem<EffectBufferSystem>();
            m_EffectCommandQueue = m_EffectCommandSystem.CreateCommandsQueue();
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public NativeMultiHashMap<ulong, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, InflictDamageEffect> RegisteredEffects;

            public ComponentDataFromEntity<Health> Healths;
            public NativeQueue<EffectCommand>.ParallelWriter EffectCommandQueue;
            public BufferFromEntity<OnDeath> OnDeathBuffer;

            public void Execute()
            {
                while(EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    Entity target = command.Target;

                    if (!Healths.Exists(target)) continue;
                   
                    Health health = Healths[target];
                    health.Value -= RegisteredEffects[command.RegistryReference.VersionId].Amount;
                    Healths[target] = health;

                    if (health.Value > 0) continue;

                    if (!OnDeathBuffer.Exists(target)) continue;
                    NativeArray<OnDeath>.Enumerator OnDeathEnum = OnDeathBuffer[target].GetEnumerator();

                    while (OnDeathEnum.MoveNext())
                    {
                        var c = new EffectCommand()
                        {
                            RegistryReference = OnDeathEnum.Current.EffectReference,
                            Emitter = command.Emitter,
                            Target = target
                        };
                        EffectCommandQueue.Enqueue(c);
                    }
                }
            }
        }
    }

}