using Wayn.Mgm.Effects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Wayn.Mgm.Effects.Generated;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
[UpdateBefore(typeof(EffectConsumerSystemGroup))]
public class CollisionTrigerHierarchyDestructionSystem : JobComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    private EffectBufferSystem m_EffectBufferSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
    }

    [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<DestroyOnCollideTag> EntitiesToDestroy;
        [ReadOnly] public ComponentDataFromEntity<DisableOnCollideTag> EntitiesToDisable;
        [ReadOnly] public ComponentDataFromEntity<Health> EntitiesWithHealth;

        public EffectBuffer EffectBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {

            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;
            
            if (EntitiesWithHealth.Exists(entityA))
                EffectBuffer.Add(4279, 0, entityA, entityB);
            if (EntitiesWithHealth.Exists(entityB))
                EffectBuffer.Add(4279, 0, entityB, entityA);

            if (EntitiesToDestroy.Exists(entityA))
                EffectBuffer.Add(4205, 0, entityA, entityA);
            if (EntitiesToDestroy.Exists(entityB))
                EffectBuffer.Add(4205, 0, entityB, entityB);

            if (EntitiesToDisable.Exists(entityA))
                EffectBuffer.Add(3514, 0, entityA, entityA);
            if (EntitiesToDisable.Exists(entityB))
                EffectBuffer.Add(3514, 0, entityA, entityA);
                
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new CollisionEventSystemJob() {
            EntitiesToDestroy = GetComponentDataFromEntity<DestroyOnCollideTag>(true),
            EntitiesToDisable = GetComponentDataFromEntity<DisableOnCollideTag>(true),
            EntitiesWithHealth = GetComponentDataFromEntity<Health>(true),
            EffectBuffer = m_EffectBufferSystem.EffectBuffer
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, 
             inputDependencies);


        m_EffectBufferSystem.AddJobHandleForConsumer(job);
        return job;
    }
}

