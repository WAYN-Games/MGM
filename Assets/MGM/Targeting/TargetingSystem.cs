using MGM.Targeting;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace MGM.Targeting
{

    public class TargetingSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

        }

        struct RemoveAllTargetJob : IJobForEachWithEntity<Target>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;


            public void Execute(Entity entity, int index, [ReadOnly] ref Target t)
            {
                CommandBuffer.RemoveComponent(index, entity, t.GetType());
            }
     
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var removeTargetJob = new RemoveAllTargetJob() { CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent() };

            var jh = removeTargetJob.Schedule(this, inputDependencies);

            m_EntityCommandBufferSystem.AddJobHandleForProducer(jh);



            return jh;
        }
    }
}