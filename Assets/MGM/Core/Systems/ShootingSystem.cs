using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class BaseShootingSystem : JobComponentSystem
    {

        private EntityQuery m_Query;
        
        protected override void OnCreate()
        {
            var queryDescription = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(ShotTrigger) },
                Options = EntityQueryOptions.FilterWriteGroup
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct BaseShotJob : IJobForEachWithEntity<ShotTrigger>
        {
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;
            [WriteOnly] public NativeQueue<Entity>.Concurrent EntitiesThatTriedToShoot;

            public void Execute(Entity entity, int index, ref ShotTrigger shotTrigger)
            {
                // Increase the cool down count
                shotTrigger.TimeSinceLastTrigger += DeltaTime;

                // Spawn object only when requested
                if (!shotTrigger.IsTriggered) return;

                // Reset the input trigger
                shotTrigger.IsTriggered = false;

                // Shoot only if cooled down
                if (shotTrigger.TimeSinceLastTrigger < shotTrigger.CoolDown) return;

                EntitiesThatTriedToShoot.Enqueue(entity);

                // Reset the cool down count
                shotTrigger.TimeSinceLastTrigger = 0;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            NativeQueue<Entity> EntitiesThatTriedToShoot = new NativeQueue<Entity>(Allocator.TempJob);

            var job = new BaseShotJob
            {
                EntitiesThatTriedToShoot = EntitiesThatTriedToShoot.ToConcurrent(),
                DeltaTime = UnityEngine.Time.deltaTime
            }.ScheduleSingle(m_Query, inputDeps);

            job.Complete();

            for (int i = 0; i < EntitiesThatTriedToShoot.Count; i++)
            {
                UnityEngine.Debug.Log("Entity " + EntitiesThatTriedToShoot.Dequeue() +" tried to shoot.");
            }
            EntitiesThatTriedToShoot.Dispose();
            return job;
        }
    }


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
                All = new ComponentType[] { typeof(ShotTrigger), typeof(ShootingCapabilityParameters), ComponentType.ReadOnly<LocalToWorld>()},
                Options = EntityQueryOptions.FilterWriteGroup
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct SingleShotJob : IJobForEachWithEntity<ShootingCapabilityParameters, LocalToWorld, ShotTrigger>
        {
            // A command buffer that support parallel writes.
            public EntityCommandBuffer.Concurrent CommandBuffer;
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;

            public void Execute(Entity entity, int index, ref ShootingCapabilityParameters shotParam,
                [ReadOnly] ref LocalToWorld location, ref ShotTrigger shotTrigger)
            {
                // Increase the cool down count
                shotTrigger.TimeSinceLastTrigger += DeltaTime;

                // Spawn object only when requested
                if (!shotTrigger.IsTriggered) return;

                // Reset the input trigger
                shotTrigger.IsTriggered = false;

                // Shoot only if cooled down
                if (shotTrigger.TimeSinceLastTrigger < shotTrigger.CoolDown) return;

                // Create teh bullet
                var instance = CommandBuffer.Instantiate(index, shotParam.spawnCapabilityParameters.Spawnable);

                // Place it at the end of the gun
                CommandBuffer.SetComponent(index, instance, new Translation { Value = location.Position });
                CommandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.LookRotationSafe(location.Forward, math.up()) });
                CommandBuffer.SetComponent(index, instance, location);
                CommandBuffer.AddComponent(index, instance, new Speed() { Value = shotParam.Speed });

                // Make it move forward
                CommandBuffer.SetComponent(index, instance, new PhysicsVelocity { Linear = location.Forward * shotParam.Speed });


                // Reset the cool down count
                shotTrigger.TimeSinceLastTrigger = 0;
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