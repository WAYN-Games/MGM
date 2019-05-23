using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

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

}