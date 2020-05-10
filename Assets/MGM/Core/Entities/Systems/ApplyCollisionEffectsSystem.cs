using Wayn.Mgm.Events;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ApplyCollisionEffectsSystem : EffectJobSystem
{
    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public BufferFromEntity<OnCollideEffectsOnOtherBuffer> EntitiesWithOnCollideEffectsOnOtherBuffer;
        [ReadOnly] public BufferFromEntity<OnCollideEffectsOnSelfBuffer> EntitiesWithOnCollideEffectsOnSelfBuffer;


        public NativeQueue<EffectCommand>.ParallelWriter EffectCommandQueue;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;
            ApplyOnCollideEffectsOnOtherBuffer(entityA, entityB);
            ApplyOnCollideEffectsOnOtherBuffer(entityB, entityA);
            ApplyOnCollideEffectsOnSelfBuffer(entityA, entityA);
            ApplyOnCollideEffectsOnSelfBuffer(entityB, entityB);

        }

        private void ApplyOnCollideEffectsOnOtherBuffer(Entity emmiter, Entity target)
        {
            if (EntitiesWithOnCollideEffectsOnOtherBuffer.Exists(emmiter))
            {
                var enumerator = EntitiesWithOnCollideEffectsOnOtherBuffer[emmiter].GetEnumerator();
                while (enumerator.MoveNext())
                {
                    EffectCommandQueue.Enqueue(new EffectCommand()
                    {
                        RegistryReference = enumerator.Current.RegistryEventReference,
                        Emitter = emmiter,
                        Target = target
                    });
                }

            }
        }
        private void ApplyOnCollideEffectsOnSelfBuffer(Entity emmiter, Entity target)
        {
            if (EntitiesWithOnCollideEffectsOnSelfBuffer.Exists(emmiter))
            {
                var enumerator = EntitiesWithOnCollideEffectsOnSelfBuffer[emmiter].GetEnumerator();
                while (enumerator.MoveNext())
                {
                    EffectCommandQueue.Enqueue(new EffectCommand()
                    {
                        RegistryReference = enumerator.Current.RegistryEventReference,
                        Emitter = emmiter,
                        Target = target
                    });
                }

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new CollisionEventSystemJob()
        {
            EntitiesWithOnCollideEffectsOnOtherBuffer = GetBufferFromEntity<OnCollideEffectsOnOtherBuffer>(true),
            EntitiesWithOnCollideEffectsOnSelfBuffer = GetBufferFromEntity<OnCollideEffectsOnSelfBuffer>(true),
            EffectCommandQueue = m_EffectBufferSystem.CreateCommandsQueue()
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld,
             inputDependencies);
        AddJobHandleForConsumer(job);
        return job;
    }



    
}
