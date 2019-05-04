using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

namespace MGM
{

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class JumpSystem : JobComponentSystem
    {
        [BurstCompile]
        struct JumpJob : IJobForEachWithEntity<WorldRenderBounds, PhysicsVelocity, JumpCapabilityParameters>
        {
            [ReadOnly] public CollisionWorld World;

            public void Execute(Entity entity, int index, [ReadOnly]ref WorldRenderBounds renderBounds, ref PhysicsVelocity physics, ref JumpCapabilityParameters jcp)
            {
                // Do nothing if we don't try to jump
                if (!jcp.JumpTrigerred) return;

                // Reset the jump trigger to prevent auto jumping.
                jcp.JumpTrigerred = false;

                // if we already jumped at least once
                if (jcp.CurrentJumpCount > 0)
                {
                    // reset jump count if we are supported
                    if (IsSupported(entity, renderBounds)) jcp.CurrentJumpCount = 0;
                }

                // If we have not reache the max jump count
                if (jcp.CurrentJumpCount < jcp.MaxJumpNumber)
                {
                    // jump !
                    physics.Linear.y = jcp.Force;
                    jcp.CurrentJumpCount++;
                }

            }

            private bool IsSupported(Entity entity, WorldRenderBounds renderBounds)
            {
                var RaycastInput = new RaycastInput
                {
                    Ray = new Unity.Physics.Ray { Origin = renderBounds.Value.Center, Direction = -math.up()*(renderBounds.Value.Extents.y+0.1f) },
                    Filter = CollisionFilter.Default
                };


                ClosestHitWithIgnoreCollector collector = new ClosestHitWithIgnoreCollector(1f, World.Bodies, entity);

                // NEED Unity Fix - Cast will return true even if the hit is the ignered entity.
                if (World.CastRay(RaycastInput, ref collector))
                {
                    // Work around to check that the hit is not the ignore entity
                    if (collector.Hit.ColliderKey.Value != 0)
                    {
                        Debug.Log("true");
                        return true;
                    }
                }

                Debug.Log("false");
                return false;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new JumpJob() { World = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld }.Schedule(this, inputDeps);
        }



    }
}