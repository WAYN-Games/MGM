using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class GridShotSystem : JobComponentSystem
    {
        private EntityQuery m_Query;
        /// <summary>
        /// System to create a command buffer.
        /// </summary>
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            // Get all entities with Shot, SingleShot and LocalToWorld components. If one entity is missing one of those component it will not be retrieved by this query.
            var queryDescription = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Shot), typeof(Magazine), ComponentType.ReadOnly <GridShot>(), ComponentType.ReadOnly<LocalToWorld>()},
                Options = EntityQueryOptions.FilterWriteGroup // Enable write groups so that the system override the default behaviour of the write group system.
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct GridShotJob : IJobForEachWithEntity<Shot, Magazine,LocalToWorld, GridShot>
        {
            // A command buffer that support parallel writes.
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, ref Shot shot, ref Magazine magazine,
                [ReadOnly] ref LocalToWorld location, [ReadOnly] ref GridShot gridShot)
            {
                // Increase the cool down count
                shot.Trigger.TimeSinceLastTrigger += DeltaTime;

                // Spawn object only when requested
                if (!shot.Trigger.IsTriggered) return;

                // Reset the input trigger
                shot.Trigger.IsTriggered = false;

                // Shoot only if cooled down
                if (shot.Trigger.TimeSinceLastTrigger < shot.Trigger.CoolDown) return;
                
                // Shoot only if projectile left.
                if (magazine.CurrentCapacity <= 0) return;

                // remove one projectile from the magazine.
                magazine.CurrentCapacity -= 1;


                float xRange = (float)gridShot.SizeX / 2;
                float yRange = (float)gridShot.SizeY / 2;
                for (float x = -xRange; x < xRange; x++)
                {
                    for (float y = -yRange; y < yRange; y++)
                    {
                        // Create the bullet
                        var instance = CommandBuffer.Instantiate(index, shot.Projectile);

                        // Place it at the end of the gun
                        CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position });
                        float3 projectileDirection = location.Position + (location.Forward * gridShot.Density) + (location.Right * (x + .5f)) + (location.Up * (y + .5f));
                        CommandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.LookRotationSafe(projectileDirection, math.up()) });
                        CommandBuffer.SetComponent(index, instance, new LocalToWorld { Value = float4x4.LookAt(location.Position, projectileDirection, math.up())});

                        // Save the speed on the projectile to avoid calculating it.
                        CommandBuffer.AddComponent(index, instance, new Speed() { Value = shot.Speed });

                        // Make it move forward
                        CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = (projectileDirection- location.Position) * shot.Speed });
                        // Gizmos.DrawLine(location.position, location.position + (transform.forward * Density) + (transform.right * (x + .5f)) + (transform.up * (y + .5f)));
                    }
                }

              



                // Reset the cool down count
                shot.Trigger.TimeSinceLastTrigger = 0;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new GridShotJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(), // Pass in the command buffer allowing the creation of new entitites and make it thread safe
                DeltaTime = UnityEngine.Time.deltaTime
            }.ScheduleSingle(m_Query, inputDeps);

            // Set the command buffer to be played back effectively executing every store command during the job.
            m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }




}