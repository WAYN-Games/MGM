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
        public void Execute(TriggerEvent triggerEvent)
        {
            //EntityCommandBuffer.AddComponent(triggerEvent.Entities.EntityA, new DestroyHierarchy());
            //EntityCommandBuffer.AddComponent(triggerEvent.Entities.EntityB, new DestroyHierarchy());
        }

       
    }

  

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new CollisionEventSystemJob() {
           EntityCommandBuffer =  m_EntityCommandBufferSystem.CreateCommandBuffer()
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, 
             inputDependencies);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }


}