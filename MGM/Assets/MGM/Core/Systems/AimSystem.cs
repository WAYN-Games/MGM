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
    public class AimSystem : JobComponentSystem
    {
        [BurstCompile]
        struct AimJob : IJobForEach<Rotation, Aim, Translation>
        {
             public void Execute(ref Rotation rotation, [ReadOnly] ref Aim aim, [ReadOnly] ref Translation position)
            {
                if (aim.Value.Equals(float3.zero)) return;
                aim.Value = aim.Value - position.Value;
                aim.Value.y = 0;
                rotation.Value = quaternion.LookRotationSafe(aim.Value, math.up());
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new AimJob().Schedule(this, inputDependencies);
        }
        
    }
}