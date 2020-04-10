using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events.Registry
{
    public abstract class RegistryEventConsumer<COMMAND,ELEMENT,REGISTRY,DISPATCHER> : JobComponentSystem
        where COMMAND : struct, IEventRegistryCommand
        where ELEMENT : struct,IRegistryElement
        where REGISTRY : Registry<REGISTRY, ELEMENT>
        where DISPATCHER : RegistryEventDispatcher<COMMAND>
    {
        private NativeMultiHashMap<ulong, COMMAND> m_CommandMap;
        private ulong m_TypeId;
        private NativeHashMap<int, ELEMENT> m_RegisteredEffects;
        private REGISTRY m_Registry;
        private DISPATCHER m_DispatcherSystem;


        private EndSimulationEntityCommandBufferSystem m_EndSimulationEntityCommandBufferSystem;
        private EntityCommandBuffer m_EntityCommandBuffer;
    }
}

namespace Wayn.Mgm.Events
{


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystem<E> : JobComponentSystem
        where E : struct, IEffect
    {
        private EffectBufferSystem m_EffectBufferSystem; //
        private EffectRegistry m_EffectRegistry; ///
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEntityCommandBufferSystem;//


        private NativeHashMap<int, E> m_RegisteredEffects;//
        private NativeMultiHashMap<ulong, EffectCommand> m_EffectCommandMap;//
        private ulong m_EffectTypeId;//
        private EntityCommandBuffer m_EntityCommandBuffer;//

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

            m_EffectCommandMap = m_EffectBufferSystem.CommandsMap;

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
