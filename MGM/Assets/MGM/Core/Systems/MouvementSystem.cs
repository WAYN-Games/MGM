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
    
    public class MouvementSystem : JobComponentSystem
    {

        [BurstCompile]
        struct MoveJob : IJobForEach<MouvementCapabilityParameters, PhysicsVelocity, Rotation>
        {
             public void Execute([ReadOnly]ref MouvementCapabilityParameters mcp, ref PhysicsVelocity physics, ref Rotation rotation)
            {
                physics.Angular = float3.zero;
                // Slow motion down based on inertia.
                if (mcp.direction.x == 0 && mcp.direction.y == 0)
                {
                    physics.Linear.x *= mcp.MovementInertia;
                    
                    physics.Linear.z *= mcp.MovementInertia;
                    return;
                }

                // Give linear velocity based on speed and input direction.
                physics.Linear.x = mcp.direction.x * mcp.Speed;
                physics.Linear.z = mcp.direction.y * mcp.Speed;
                
                // Rotate to face the movement direction.
                if (mcp.ShouldFaceForward) { 
                    rotation.Value = quaternion.LookRotationSafe(new float3() { x = mcp.direction.x , z = mcp.direction.y }, math.up());
                }
                mcp.direction = float2.zero;
            }


        }


        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return new MoveJob().Schedule(this, inputDependencies);
        }
        
    }
}