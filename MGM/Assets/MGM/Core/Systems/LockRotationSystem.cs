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
    public class LockRotationSystem : JobComponentSystem
    {

        [BurstCompile]
        [RequireComponentTag(typeof(LockRotationTag))]
        struct LockRotationJob : IJobForEach<Rotation>
        {
             public void Execute(ref Rotation rotation)
            {
                rotation.Value = quaternion.identity;
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new LockRotationJob().Schedule(this, inputDependencies);
        }
        
    }
}