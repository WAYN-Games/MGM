using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Effects
{
    public abstract class ConsumerSystem : JobComponentSystem
    {
    }

    public abstract class ConsumerSystem<J, E> : ConsumerSystem
        where J : struct, IJob
        where E : struct, IEffect
    {
        /// <summary>
        /// Represent the las system of the simulation loop.
        /// </summary>
        protected EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        /// <summary>
        /// Store all the effect to apply.
        /// </summary>
        protected NativeQueue<E> ConsumerQueue;

        protected EffectBufferSystem m_EffectBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            ConsumerQueue = new NativeQueue<E>(Allocator.Persistent);
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
        }

        public NativeQueue<E>.ParallelWriter GetConsumerQueue()
        {
            return ConsumerQueue.AsParallelWriter();
        }



        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // Schedule the job as soon as all effect initiators have completed and
            JobHandle jh = CreateJob().Schedule(JobHandle.CombineDependencies(m_EffectBufferSystem.GetJobHandle(), inputDeps));

            // Force the job to complete before the last system of the simulation loop.
            // This is to make sure the job complete as late as possible while not allowing it to late longer than a frames.
            // If it lasted longer than a frame, the ConsumerQueue could be written to while still being consumed.
            m_EntityCommandBufferSystem.AddJobHandleForProducer(jh);

            return jh;
        }

        protected abstract J CreateJob();
    }

}
