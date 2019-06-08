using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(AgingSystem))]
    public class DieOfOldAgeSystem : JobComponentSystem
    {
       
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
                m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct DieOfOldAgeJob : IJobForEachWithEntity<CurrentAge, MaxAge>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref CurrentAge currentAge,
                [ReadOnly] ref MaxAge maxAge)
            {
                if (currentAge.Value < maxAge.Value) return;
                CommandBuffer.DestroyEntity(index,entity);
                
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