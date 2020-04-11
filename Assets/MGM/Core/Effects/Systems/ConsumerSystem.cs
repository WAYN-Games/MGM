using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystem<E> : JobComponentSystem
        where E : struct, IEffect
    {
        private EffectBufferSystem m_EffectBufferSystem; 
        private EffectRegistry m_EffectRegistry;


        private NativeHashMap<int, E> m_RegisteredEffects;
        private UnsafeMultiHashMap<ulong, EffectCommand> m_EffectCommandMap;
        
        private EntityCommandBuffer m_EntityCommandBuffer;

        private bool ShouldRefreshCache = true;

        private ulong m_EffectTypeId;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectTypeId = RegistryReference.GetTypeId(typeof(E));

         

            RefreshRegisteredEffectsCache();
            m_EffectRegistry.NewEffectRegisteredEvent += ()=> ShouldRefreshCache = true;

            m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
            
        }
     
        private void RefreshRegisteredEffectsCache()
        {
            m_EffectRegistry = EffectRegistry.Instance;

            if (m_RegisteredEffects.IsCreated) m_RegisteredEffects.Dispose();
        
            m_EffectRegistry.GetRegisteredEffects(ref m_RegisteredEffects);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_EffectRegistry.NewEffectRegisteredEvent -= RefreshRegisteredEffectsCache;
            m_RegisteredEffects.Dispose();
        }

        protected abstract JobHandle ScheduleJob(
            in JobHandle inputDeps,
            in UnsafeMultiHashMap<ulong,EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, E> RegisteredEffects);

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ShouldRefreshCache)
            {
                RefreshRegisteredEffectsCache();
                ShouldRefreshCache = false;
            }
           
            m_EffectCommandMap = m_EffectBufferSystem.CommandsMap;


            JobHandle dependencies = JobHandle.CombineDependencies(m_EffectBufferSystem.FinalJobHandle, inputDeps);
            var effectCommandEnumerator = m_EffectCommandMap.GetValuesForKey(m_EffectTypeId);
            JobHandle ExecuteEffectCommandsJob = ScheduleJob(
                in dependencies,
                in effectCommandEnumerator,
                in m_RegisteredEffects);
            m_EffectBufferSystem.AddConsumerJobHandle(ExecuteEffectCommandsJob);
         
            return ExecuteEffectCommandsJob;
        }

    }
}
