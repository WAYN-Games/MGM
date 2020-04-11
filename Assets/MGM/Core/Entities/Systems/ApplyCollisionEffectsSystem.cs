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
        public NativeQueue<EffectCommand>.ParallelWriter EffectCommandQueue2;
        public NativeQueue<EffectCommand>.ParallelWriter EffectCommandQueue3;

        private int loopcount;

        public void Execute(TriggerEvent triggerEvent)
        {
            loopcount = 1000;
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
                    var c = new EffectCommand()
                    {
                        RegistryReference = enumerator.Current.EffectReference,
                        Emitter = emmiter,
                        Target = target
                    };
                    for (int i = 0; i < loopcount; i++)
                    {
                        EffectCommandQueue.Enqueue(c);
                        EffectCommandQueue2.Enqueue(c);
                        EffectCommandQueue3.Enqueue(c);
                    }
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
                    var c = new EffectCommand()
                    {
                        RegistryReference = enumerator.Current.EffectReference,
                        Emitter = emmiter,
                        Target = target
                    };
                    for (int i = 0; i < loopcount; i++)
                    {
                        EffectCommandQueue.Enqueue(c);
                        EffectCommandQueue2.Enqueue(c);
                        EffectCommandQueue3.Enqueue(c);
                    }
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
            EffectCommandQueue = m_EffectCommandQueue,
            EffectCommandQueue2 = m_EffectCommandQueue2,
            EffectCommandQueue3 = m_EffectCommandQueue3
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld,
             inputDependencies);
        AddJobHandleForConsumer(job);
        return job;
    }


}
