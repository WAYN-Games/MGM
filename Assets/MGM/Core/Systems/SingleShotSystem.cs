using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SingleShotSystem : JobComponentSystem
    {
        private EntityQuery m_Query;
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            var queryDescription = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Shot), ComponentType.ReadOnly <SingleShot>(), ComponentType.ReadOnly<LocalToWorld>()},
                Options = EntityQueryOptions.FilterWriteGroup
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct SingleShotJob : IJobForEachWithEntity<Shot, LocalToWorld, SingleShot>
        {
            // A command buffer that support parallel writes.
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, ref Shot shot,
                [ReadOnly] ref LocalToWorld location, [ReadOnly] ref SingleShot SingleShot)
            {
                // Increase the cool down count
                shot.Trigger.TimeSinceLastTrigger += DeltaTime;

                // Spawn object only when requested
                if (!shot.Trigger.IsTriggered) return;

                // Reset the input trigger
                shot.Trigger.IsTriggered = false;

                // Shoot only if cooled down
                if (shot.Trigger.TimeSinceLastTrigger < shot.Trigger.CoolDown) return;

                // Create teh bullet
                var instance = CommandBuffer.Instantiate(index, shot.Projectile);

                // Place it at the end of the gun
                CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position });
                CommandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.LookRotationSafe(location.Forward, math.up()) });
                CommandBuffer.SetComponent(index, instance, location);

                // Save the speed on the projectile to avoid calculating it.
                CommandBuffer.AddComponent(index, instance, new Speed() { Value = shot.Speed });

                // Make it move forward
                CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = location.Forward * shot.Speed });


                // Reset the cool down count
                shot.Trigger.TimeSinceLastTrigger = 0;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new SingleShotJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(), // Pass in the command buffer allowing the creation of new entitites
                DeltaTime = UnityEngine.Time.deltaTime
            }.ScheduleSingle(m_Query, inputDeps);


            m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }




}