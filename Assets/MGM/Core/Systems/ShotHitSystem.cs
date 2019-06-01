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
    public class DestroyOnCollisionSystem : JobComponentSystem
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
        struct DetectCollisionJob : IJobForEachWithEntity<Speed, Damage, LocalToWorld>
        {
            [WriteOnly] public NativeQueue<Entity>.Concurrent BulletsThatCollided;
            [WriteOnly] public NativeQueue<DealDamageTo>.Concurrent EntitiesHitByBullet;
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public CollisionWorld World;

            public void Execute(Entity entity, int index, [ReadOnly] ref Speed speed, [ReadOnly] ref Damage damage,
                [ReadOnly] ref LocalToWorld localToWorld)
            {
                var RaycastInput = new RaycastInput
                {
                    Start = localToWorld.Position,
                    End = localToWorld.Position + localToWorld.Forward * DeltaTime * speed.Value,
                    Filter = CollisionFilter.Default
                };


                if (World.CastRay(RaycastInput,out Unity.Physics.RaycastHit hit))
                {
                    BulletsThatCollided.Enqueue(entity);
                    DealDamageTo ddt = new DealDamageTo()
                    {
                        Entity = World.Bodies[hit.RigidBodyIndex].Entity,
                        Amount = damage.Value
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

        struct DestroyOnImapct : IJobParallelFor
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            [ReadOnly] public NativeQueue<Entity> EntitiesToDestroy;

            public void Execute(int index)
            {
               CommandBuffer.DestroyEntity(index, EntitiesToDestroy.Peek());
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
       
            NativeQueue<Entity> BulletsThatCollided = new NativeQueue<Entity>(Allocator.TempJob);
            NativeQueue<DealDamageTo> DamageDoneToEntitiesHitByBullet = new NativeQueue<DealDamageTo>(Allocator.TempJob);

            var detectionJob = new DetectCollisionJob
            {
                DeltaTime = Time.deltaTime,
                BulletsThatCollided = BulletsThatCollided.ToConcurrent(),
                EntitiesHitByBullet = DamageDoneToEntitiesHitByBullet.ToConcurrent(),
                World = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld
            }.Schedule(this, inputDeps);

            detectionJob.Complete();

            
            var destroyBulletJob = new DestroyOnImapct
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                EntitiesToDestroy = BulletsThatCollided
            }.Schedule(BulletsThatCollided.Count, 64 ,detectionJob);
            /*
            m_EntityCommandBufferSystem.AddJobHandleForProducer(destroyBulletJob);
            */
            destroyBulletJob.Complete();

            // Temp fix because Collection support for paralelism is to weak now. Lose perf but at least it works as expected.
            while (BulletsThatCollided.Count != 0)
            {
                EntityManager.DestroyEntity(BulletsThatCollided.Dequeue());
            }

            var applyDamageJob = new ApplyDamage
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                DamageToApplyTo = DamageDoneToEntitiesHitByBullet
            }.Schedule(this, destroyBulletJob);

            applyDamageJob.Complete();

            BulletsThatCollided.Dispose();
            DamageDoneToEntitiesHitByBullet.Dispose();

            return applyDamageJob;
        }
    }
}