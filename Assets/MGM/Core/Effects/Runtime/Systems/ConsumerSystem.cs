using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public abstract class EffectConsumerSystem<E> : JobComponentSystem
        where E : struct, IEffect
    {
        private EffectBufferSystem m_EffectBufferSystem; 
        private EffectRegistry m_EffectRegistry;


        private NativeHashMap<int, E> m_RegisteredEffects;
        private NativeMultiHashMap<MapKey, EffectCommand> m_EffectCommandMap;
        
        private EntityCommandBuffer m_EntityCommandBuffer;

        private bool ShouldRefreshCache = true;

        private int m_EffectTypeId;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
            m_EffectTypeId = RegistryReference.GetTypeId(typeof(E));

            RefreshRegisteredEffectsCache();
            m_EffectRegistry.NewEffectRegisteredEvent += ()=> ShouldRefreshCache = true;

            
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
            JobHandle inputDeps,
            in NativeMultiHashMap<MapKey,EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, E> RegisteredEffects);

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ShouldRefreshCache)
            {
                RefreshRegisteredEffectsCache();
                ShouldRefreshCache = false;
            }
           
            m_EffectCommandMap = m_EffectBufferSystem.CommandsMap;
            var effectCommandEnumerator = m_EffectCommandMap.GetValuesForKey(new MapKey() { Value = m_EffectTypeId });
            JobHandle ExecuteEffectCommandsJob = ScheduleJob(
                JobHandle.CombineDependencies(inputDeps,m_EffectBufferSystem.finalJobHandle),
                in effectCommandEnumerator,
                in m_RegisteredEffects);
            m_EffectBufferSystem.AddConsumerJobHandle(ExecuteEffectCommandsJob);
         
            return ExecuteEffectCommandsJob;
        }

    }
}
