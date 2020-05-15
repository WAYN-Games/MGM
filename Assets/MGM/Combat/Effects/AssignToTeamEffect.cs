using System;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

using Wayn.Mgm.Event;
using Wayn.Mgm.Event.Registry;

namespace Wayn.Mgm.Combat.Effects
{
    [Serializable]
    public struct AssignToTeamEffect : IEffect
    {
        public AssignementStrategy AssignementStrategy;
    }

    public enum AssignementStrategy
    {
        FewestMembers,
        LosingTeam
    }

    public class AssignToTeamEffectConsumer : EffectConsumerSystem<AssignToTeamEffect>
    {
        private EffectDisptacherSystem m_EffectCommandSystem;


        protected override JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, AssignToTeamEffect> RegisteredEffects)
        {
            JobHandle jh = new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                RegisteredEffects = RegisteredEffects
            }.Schedule(Dependency);

            m_EffectCommandSystem.AddJobHandleFromProducer(jh);
            return jh;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectCommandSystem = World.GetOrCreateSystem<EffectDisptacherSystem>();
        }


        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public NativeHashMap<int, AssignToTeamEffect> RegisteredEffects;

            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    EffectCommand command = EffectCommandEnumerator.Current;
                    Entity target = command.Target;
                }
            }
        }
    }

}
