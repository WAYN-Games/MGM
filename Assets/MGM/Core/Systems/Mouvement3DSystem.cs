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
    public class Mouvement3DSystem : JobComponentSystem
    {

        [BurstCompile]
        [RequireComponentTag(typeof(Mouvement3DSystemTarget))]
        public struct Move3DJob : IJobForEach<MouvementCapabilityParameters, PhysicsVelocity, Rotation, Heading>
        {
            [ReadOnly] public Vector3 camForward;
            [ReadOnly] public Vector3 camRight;

            public void Execute([ReadOnly]ref MouvementCapabilityParameters mcp, ref PhysicsVelocity physics, ref Rotation rotation, ref Heading heading)
            {
                physics.Angular = float3.zero;
                
                if (heading.Value.x == 0 && heading.Value.z == 0)
                {
                    // Slow motion down based on inertia.
                    physics.Linear *= mcp.MovementInertia;
                }
                else {

                    var desiredMoveDirection = camRight * heading.Value.x  + camForward * heading.Value.z ;

                    physics.Linear = desiredMoveDirection * mcp.Speed;

                    // Rotate to face the movement direction.
                    if (mcp.ShouldFaceForward)                    {
                        rotation.Value = quaternion.LookRotationSafe(new float3() { x = desiredMoveDirection.x, z = desiredMoveDirection.z }, math.up());
                    }
                }

                //heading.Value = float3.zero;
            }

        }


        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            return new Move3DJob() { camForward = camForward, camRight = camRight }.Schedule(this, inputDependencies);
        }
    
    }

}