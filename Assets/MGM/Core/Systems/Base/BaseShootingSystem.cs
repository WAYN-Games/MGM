using System;
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
        protected EntityQuery m_Query;
        
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



                // Reset the cool down count
                shot.Trigger.TimeSinceLastTrigger = 0;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            
            var job = new BaseShotJob
            {
                DeltaTime = UnityEngine.Time.deltaTime
            }.ScheduleSingle(m_Query, inputDeps);

            return job;
        }
    }

}