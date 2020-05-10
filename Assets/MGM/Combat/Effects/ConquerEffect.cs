using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect add the Amount the the Target entity's Health
    /// </summary>
    [Serializable]
    public struct ConquerEffect : IEffect
    {

        /// <summary>
        /// The amount of ownerqhip changed per second.
        /// </summary>
        public float Amount;

    }

    public class ConquerEffectConsumer : EffectConsumerSystem<ConquerEffect>
    {
        private EffectDisptacherSystem m_EffectCommandSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectCommandSystem = World.GetOrCreateSystem<EffectDisptacherSystem>();
        }

        protected override JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, ConquerEffect> RegisteredEffects)
        {
            var jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects,
                OwnershipPoints = GetComponentDataFromEntity<OwnershipPoint>(false),
                Time = Time.DeltaTime
            }.Schedule(Dependency);

            m_EffectCommandSystem.AddJobHandleFromProducer(jh);
            return jh;
        }

        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, ConquerEffect> RegisteredEffects;
            [ReadOnly]
            public float Time;

            public ComponentDataFromEntity<OwnershipPoint> OwnershipPoints;

            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    Entity target = command.Target;

                    if (!OwnershipPoints.Exists(target)) continue;

                    // != teams
                    //OwnershipPoints[target] = PoolMethods.SubtractValue(OwnershipPoints[target], RegisteredEffects[command.RegistryReference.VersionId].Amount * Time);

                    // == teams
                    OwnershipPoints[target] = PoolMethods.AddValue(OwnershipPoints[target], RegisteredEffects[command.RegistryReference.VersionId].Amount * Time);

                    if (OwnershipPoints[target].Value > 0) continue;

                    // Switch team
                  
                }
            }
        }
    }

}