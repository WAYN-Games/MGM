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
                All = new ComponentType[] { typeof(Shot), typeof(Magazine), typeof(SoundFX),ComponentType.ReadOnly <GridShot>(), ComponentType.ReadOnly<LocalToWorld>()},
                Options = EntityQueryOptions.FilterWriteGroup // Enable write groups so that the system override the default behaviour of the write group system.
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct GridShotJob : IJobForEachWithEntity<Shot, Magazine, SoundFX,LocalToWorld, GridShot>
        {
            // A command buffer that support parallel writes.
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, ref Shot shot, ref Magazine magazine, ref SoundFX sfx,
                [ReadOnly] ref LocalToWorld location, [ReadOnly] ref GridShot gridShot)
            {
                if(!Shot.IsTriggered(ref shot, DeltaTime)) return;

                if (Magazine.IsMagazineEmpty(ref magazine)) return;

                //Normalize verctor to match drawn gizmos and be independant of scale.
                float3 normalizedForward = math.normalizesafe(location.Forward);
                float3 normalizedRight = math.normalizesafe(location.Right);
                float3 normalizedUp = math.normalizesafe(location.Up);

                float xRange = (float)gridShot.SizeX / 2;
                float yRange = (float)gridShot.SizeY / 2;
                for (float x = -xRange; x < xRange; x++)
                {
                    for (float y = -yRange; y < yRange; y++)
                    {
                        // Create the bullet
                        var instance = CommandBuffer.Instantiate(index, shot.Projectile);

                        // Place it at the end of the gun


                        CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position});
                        float3 projectileDirection = (normalizedForward * gridShot.Density) + (normalizedRight * (x + .5f)) + (normalizedUp * (y + .5f));
                        CommandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.LookRotationSafe(projectileDirection, normalizedUp) });
                        CommandBuffer.SetComponent(index, instance, new LocalToWorld { Value = float4x4.LookAt(location.Position, projectileDirection, normalizedUp) });

                        // Save the speed on the projectile to avoid calculating it.
                        CommandBuffer.AddComponent(index, instance, new Speed() { Value = shot.Speed });

                        // Make it move forward
                        CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = (projectileDirection) * shot.Speed / gridShot.Density });
                    }
                }



                SoundFX.PlaySFXAt(ref sfx, location.Position);
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