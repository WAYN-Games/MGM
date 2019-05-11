using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SpawnSystem : JobComponentSystem
    {
       
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
                m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct SpawnJob : IJobForEachWithEntity<SpawnCapabilityParameters, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, [ReadOnly] ref SpawnCapabilityParameters spawner,
                [ReadOnly] ref LocalToWorld location)
            {
                // Increase the cool down count
                spawner.TimeSinceLastTrigger += DeltaTime;
                
                // Spawn only if cooled down
                if (spawner.TimeSinceLastTrigger < spawner.CoolDown) return;

                // Create the spawnable
                var instance = CommandBuffer.Instantiate(index, spawner.Spawnable);

                // Place it at the end of the gun
                CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position });

                // Reset the cool down count
                spawner.TimeSinceLastTrigger = 0;
            }

            
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new SpawnJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(), // Pass in the command buffer allowing the creation of new entitites
                DeltaTime = UnityEngine.Time.deltaTime
            }.Schedule(this, inputDeps);


            m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}