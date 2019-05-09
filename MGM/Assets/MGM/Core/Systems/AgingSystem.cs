using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Systems;

namespace MGM
{

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class AgingSystem : JobComponentSystem
    {
        [BurstCompile]
        struct AgingJob : IJobForEach<CurrentAge>
        {
            [ReadOnly] public float DeltaTime;

             public void Execute(ref CurrentAge currentAge)
            {
                currentAge.Value += DeltaTime;
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new AgingJob() { DeltaTime  = Time.deltaTime}.Schedule(this, inputDependencies);
        }
        
    }
}