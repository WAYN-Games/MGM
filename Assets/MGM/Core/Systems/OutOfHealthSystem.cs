using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

namespace MGM
{
    

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class OutOfHealth : JobComponentSystem
    {


        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct DieOfOldAgeJob : IJobForEachWithEntity<Health>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref Health health)
            {
                if (health.Value > 0) return;
                CommandBuffer.DestroyEntity(index, entity);

            }


        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new DieOfOldAgeJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);


            m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }



    }
}