using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using Unity.Physics;

namespace MGM
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class MouvementSystem : JobComponentSystem
    {

        [BurstCompile]
        [RequireComponentTag(typeof(Mouvement2DSystemTarget))]
        public struct MoveJob : IJobForEach<MouvementCapabilityParameters, PhysicsVelocity, Rotation, Heading>
        {
            public void Execute([ReadOnly]ref MouvementCapabilityParameters mcp, ref PhysicsVelocity physics, ref Rotation rotation, ref Heading heading)
            {
                physics.Angular = float3.zero;
                
                if (heading.Value.x == 0 && heading.Value.z == 0)
                {
                    // Slow motion down based on inertia.
                    physics.Linear.x *= mcp.MovementInertia;
                    physics.Linear.z *= mcp.MovementInertia;
                }
                else { 
                    // Give linear velocity based on speed and input direction.
                    physics.Linear.x = heading.Value.x * mcp.Speed;
                    physics.Linear.z = heading.Value.z * mcp.Speed;

                    // Rotate to face the movement direction.
                    if (mcp.ShouldFaceForward)                    {
                        rotation.Value = quaternion.LookRotationSafe(new float3() { x = heading.Value.x, z = heading.Value.z }, math.up());
                    }
                }

                heading.Value = float3.zero;
            }

        }
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new MoveJob().Schedule(this, inputDependencies);
        }
    
    }

}