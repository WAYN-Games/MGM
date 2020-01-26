using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionTrigerHierarchyDestructionSystem : JobComponentSystem
{


    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public EntityCommandBuffer EntityCommandBuffer;

        [ReadOnly] public ComponentDataFromEntity<DestroyOnCollideTag> EntitiesToDestroy;
        [ReadOnly] public ComponentDataFromEntity<DisableOnCollideTag> EntitiesToDisable;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            if (EntitiesToDestroy.Exists(entityA)) EntityCommandBuffer.AddComponent(entityA, new DestroyHierarchy());
            if (EntitiesToDestroy.Exists(entityB)) EntityCommandBuffer.AddComponent(entityB, new DestroyHierarchy());
            if (EntitiesToDisable.Exists(entityA)) EntityCommandBuffer.AddComponent(entityA, new DisableHierarchy());
            if (EntitiesToDisable.Exists(entityB)) EntityCommandBuffer.AddComponent(entityB, new DisableHierarchy());
        }

       
    }

  

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new CollisionEventSystemJob() {
           EntityCommandBuffer =  m_EntityCommandBufferSystem.CreateCommandBuffer(),
            EntitiesToDestroy = GetComponentDataFromEntity<DestroyOnCollideTag>(true),
            EntitiesToDisable = GetComponentDataFromEntity<DisableOnCollideTag>(true)

        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, 
             inputDependencies);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }


}