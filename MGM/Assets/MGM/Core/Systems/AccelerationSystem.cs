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
    public class AccelerationSystem : JobComponentSystem
    {
        [BurstCompile]
        struct LockRotationJob : IJobForEach<PhysicsVelocity, Acceleration>
        {
             public void Execute(ref PhysicsVelocity  velocity,[ReadOnly] ref Acceleration acceleration)
            {
                velocity.Linear *= 1 + acceleration.Linear;
                velocity.Angular *= 1 + acceleration.Angular;
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new LockRotationJob().Schedule(this, inputDependencies);
        }
        
    }
}