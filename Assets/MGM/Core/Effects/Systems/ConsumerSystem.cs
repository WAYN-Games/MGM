using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Effects
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystem<E> : JobComponentSystem
        where E : struct, IEffect
    {
        private EffectBufferSystem m_EffectBufferSystem;
        private EffectRegistry m_EffectRegistry;
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEntityCommandBufferSystem;
        private NativeHashMap<int, E> m_RegisteredEffects;
        private NativeMultiHashMap<ulong, EffectCommand> m_EffectCommandMap;
        private ulong m_EffectTypeId;
        private EntityCommandBuffer m_EntityCommandBuffer;

        private bool ShouldRefreshCache = true;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EffectTypeId = RegistryReference.GetTypeId(typeof(E));

         

            RefreshRegisteredEffectsCache();
            m_EffectRegistry.NewEffectRegisteredEvent += ()=> ShouldRefreshCache = true;

            m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
            m_EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            
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
            in ulong EffectTypeId,
            in NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap,
            in NativeHashMap<int, E> RegisteredEffects,
            ref EntityCommandBuffer EntityCommandBuffer);

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (ShouldRefreshCache)
            {
                RefreshRegisteredEffectsCache();
                ShouldRefreshCache = false;
            }
           

            m_EntityCommandBuffer = m_EndSimulationEntityCommandBufferSystem.CreateCommandBuffer();

            m_EffectCommandMap = m_EffectBufferSystem.EffectCommandMap;

            JobHandle dependencies = JobHandle.CombineDependencies(m_EffectBufferSystem.FinalJobHandle, inputDeps);

            JobHandle ExecuteEffectCommandsJob = ScheduleJob(
                in dependencies,
                in m_EffectTypeId,
                in m_EffectCommandMap,
                in m_RegisteredEffects,
                ref m_EntityCommandBuffer);
            
            m_EndSimulationEntityCommandBufferSystem.AddJobHandleForProducer(ExecuteEffectCommandsJob);
            
            return ExecuteEffectCommandsJob;
        }
    }
}
