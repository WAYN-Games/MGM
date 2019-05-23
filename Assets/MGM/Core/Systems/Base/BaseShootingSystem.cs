using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
namespace MGM.Core
{
    /// <summary>
    /// A basic shooting system that will log the entity that shot a projectile.
    /// </summary>
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class BaseShootingSystem : JobComponentSystem
    {

        private EntityQuery m_Query;
        
        protected override void OnCreate()
        {
            // Get all entity with a Shot component and without any other component of the same write group.
            var queryDescription = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Shot) },
                Options = EntityQueryOptions.FilterWriteGroup
            };
            m_Query = GetEntityQuery(queryDescription);
        }

        struct BaseShotJob : IJobForEachWithEntity<Shot>
        {
            // Time since the last frame.
            [ReadOnly] public float DeltaTime;
            // Queue to store all entity that tryed to shoot so taht they can be logged after teh job.
            [WriteOnly] public NativeQueue<Entity>.Concurrent EntitiesThatTriedToShoot;

            public void Execute(Entity entity, int index, ref Shot shot )
            {
                // Increase the cool down count
                shot.Trigger.TimeSinceLastTrigger += DeltaTime;

                // Spawn object only when requested
                if (!shot.Trigger.IsTriggered) return;

                // Reset the input trigger
                shot.Trigger.IsTriggered = false;

                // Shoot only if cooled down
                if (shot.Trigger.TimeSinceLastTrigger < shot.Trigger.CoolDown) return;

                EntitiesThatTriedToShoot.Enqueue(entity);

                // Reset the cool down count
                shot.Trigger.TimeSinceLastTrigger = 0;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            // Queue to store all entity that tryed to shoot so taht they can be logged after teh job.
            NativeQueue<Entity> EntitiesThatTriedToShoot = new NativeQueue<Entity>(Allocator.TempJob);

            var job = new BaseShotJob
            {
                EntitiesThatTriedToShoot = EntitiesThatTriedToShoot.ToConcurrent(), // Make the queue thread safe
                DeltaTime = UnityEngine.Time.deltaTime
            }.ScheduleSingle(m_Query, inputDeps);

            job.Complete(); // Wait for the job top be completed in order to be able to use teh queue.

            // Log all entities taht shot.
            for (int i = 0; i < EntitiesThatTriedToShoot.Count; i++)
            {
                UnityEngine.Debug.Log("Entity " + EntitiesThatTriedToShoot.Dequeue() +" tried to shoot.");
            }
            // Dispose of the queue to free memory.
            EntitiesThatTriedToShoot.Dispose();
            return job;
        }
    }

}