using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SingleShotSystem : BaseShootingSystem
    {
        /// <summary>
        /// System to create a command buffer.
        /// </summary>
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
 

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            // Get all entities with Shot, SingleShot and LocalToWorld components. If one entity is missing one of those component it will not be retrieved by this query.
            EntityQueryDesc queryDescription = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Shot), typeof(Magazine),typeof(SoundFX), ComponentType.ReadOnly<SingleShot>(), ComponentType.ReadOnly<LocalToWorld>()},
                Options = EntityQueryOptions.FilterWriteGroup // Enable write groups so that the system override the default behaviour of the write group system.
            };
            
            m_Query = GetEntityQuery(queryDescription);


            

        }
        [BurstCompile]
        struct SingleShotJob : IJobForEachWithEntity<Shot, Magazine, SoundFX,LocalToWorld, SingleShot>
        {
            // A command buffer that support parallel writes.
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, ref Shot shot, ref Magazine magazine,ref SoundFX sfx,
                [ReadOnly] ref LocalToWorld location, [ReadOnly] ref SingleShot singleShot)
            {
                // If the pleayer did not shoot or the cool down has not expired, don't do anything.
                if (!Shot.IsTriggered(ref shot, DeltaTime)) return;

                // If the magazine is empty, don't do anything.
                if (Magazine.IsMagazineEmpty(ref magazine)) return;
              

                // Create the bullet
                var instance = CommandBuffer.Instantiate(index, shot.Projectile);

                // Place it at the end of the gun
                CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position });
                CommandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.LookRotationSafe(location.Forward, math.up()) });
                CommandBuffer.SetComponent(index, instance, location);

                // Save the speed on the projectile to avoid calculating it.
                CommandBuffer.AddComponent(index, instance, new Speed() { Value = shot.Speed });

                // Make it move forward
                CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = location.Forward * shot.Speed });

                SoundFX.PlaySFXAt(ref sfx, location.Position);
            }

            
        }

   

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
         

            var job = new SingleShotJob
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