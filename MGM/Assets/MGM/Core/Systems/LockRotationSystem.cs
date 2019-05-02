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
    public class LockRotationSystem : JobComponentSystem
    {

        [BurstCompile]
        struct LockRotationJob : IJobForEach<PhysicsMass, RotationLock>
        {
             public void Execute(ref PhysicsMass  mass,[ReadOnly] ref RotationLock axis)
            {

                if (axis.AxisLocks.x)
                    mass.InverseInertia[0] = 0;

                if (axis.AxisLocks.y)
                    mass.InverseInertia[1] = 0;

                if (axis.AxisLocks.z)
                    mass.InverseInertia[2] = 0;
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new LockRotationJob().Schedule(this, inputDependencies);
        }
        
    }
}