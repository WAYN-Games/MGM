using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ShotHitSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;


        struct DealDamageTo
        {
            public Entity Entity;
            public int Amount;
        }

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        [RequireComponentTag(typeof(DestroyOnColision))]
        struct DetectCollisionJob : IJobForEachWithEntity<PhysicsVelocity, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            [WriteOnly] public NativeQueue<DealDamageTo>.Concurrent EntitiesHitByBullet;
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public CollisionWorld World;

            public void Execute(Entity entity, int index, [ReadOnly] ref PhysicsVelocity physicsVelocity,
                [ReadOnly] ref LocalToWorld localToWorld)
            {
                float speed = math.sqrt(physicsVelocity.Linear.x * physicsVelocity.Linear.x + physicsVelocity.Linear.y * physicsVelocity.Linear.y + physicsVelocity.Linear.z * physicsVelocity.Linear.z);

                var RaycastInput = new RaycastInput
                {
                    Ray = new Unity.Physics.Ray { Origin = localToWorld.Position, Direction = - physicsVelocity.Linear * DeltaTime  },
                    Filter = CollisionFilter.Default
                };


                if (World.CastRay(RaycastInput,out Unity.Physics.RaycastHit hit))
                {
                    CommandBuffer.DestroyEntity(index, entity);
                    DealDamageTo ddt = new DealDamageTo()
                    {
                        Entity = World.Bodies[hit.RigidBodyIndex].Entity,
                        Amount = 1
                    };
                    EntitiesHitByBullet.Enqueue(ddt);
                }

            }


        }

        struct ApplyDamage : IJobForEachWithEntity<Health>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            [ReadOnly] public NativeQueue<DealDamageTo> DamageToApplyTo;

            public void Execute(Entity entity, int index, ref Health hp)
            {
                for (int i = 0; i < DamageToApplyTo.Count; i++)
                {
                   DealDamageTo ddt = DamageToApplyTo.Peek();
                   if (ddt.Entity.Equals(entity))
                    {
                        hp.Value -= ddt.Amount;                        
                    }
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
       
   
            NativeQueue<DealDamageTo> DamageDoneToEntitiesHitByBullet = new NativeQueue<DealDamageTo>(Allocator.TempJob);

            var detectionJob = new DetectCollisionJob
            {
                DeltaTime = Time.deltaTime,
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                EntitiesHitByBullet = DamageDoneToEntitiesHitByBullet.ToConcurrent(),
                World = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld
            }.Schedule(this, inputDeps);


            detectionJob.Complete();

            
       

            var applyDamageJob = new ApplyDamage
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                DamageToApplyTo = DamageDoneToEntitiesHitByBullet
            }.Schedule(this, detectionJob);

            applyDamageJob.Complete();

            DamageDoneToEntitiesHitByBullet.Dispose();

            return applyDamageJob;
        }
    }
}