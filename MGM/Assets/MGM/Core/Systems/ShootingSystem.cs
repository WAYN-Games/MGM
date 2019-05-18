using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ShootingSystem : JobComponentSystem
    {
       
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
                m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        struct SpawnJob : IJobForEachWithEntity<ShootingCapabilityParameters, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, [ReadOnly] ref ShootingCapabilityParameters shotParam,
                [ReadOnly] ref LocalToWorld location)
            {
                // Increase the cool down count
                shotParam.spawnCapabilityParameters.TimeSinceLastTrigger += DeltaTime;
                
                // Spawn object only when requested
                if (!shotParam.spawnCapabilityParameters.SpawnTrigerred) return;

                // Reset the input trigger
                shotParam.spawnCapabilityParameters.SpawnTrigerred = false;

                // Shoot only if cooled down
                if (shotParam.spawnCapabilityParameters.TimeSinceLastTrigger < shotParam.spawnCapabilityParameters.CoolDown) return;

                // Create teh bullet
                var instance = CommandBuffer.Instantiate(index, shotParam.spawnCapabilityParameters.Spawnable);

                // Place it at the end of the gun
                CommandBuffer.SetComponent(index, instance, new LocalToWorld { Value = location.Value });

                // Make it move forward
                CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = location.Forward * shotParam.Speed });

             
                // Reset the cool down count
                shotParam.spawnCapabilityParameters.TimeSinceLastTrigger = 0;
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