using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class DestroyOnCollisionSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        BuildPhysicsWorld m_BuildPhysicsWorldSystem;
        StepPhysicsWorld m_StepPhysicsWorldSystem;

        EntityQuery JobGroup;


        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
 
            m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
            JobGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Damage) }
            });
        }

        struct ProjectileDamageJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<Damage> DamageDealer;
            public ComponentDataFromEntity<Health> Target;
            public EntityCommandBuffer CommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {

                Entity entityA = triggerEvent.Entities.EntityA;
                Entity entityB = triggerEvent.Entities.EntityB;

                bool isBodyAProjectile = DamageDealer.Exists(entityA);
                bool isBodyBProjectile = DamageDealer.Exists(entityB);

                // Ignoring Triggers overlapping other Triggers
                if (isBodyAProjectile && isBodyBProjectile)
                    return;

                var projectileEntity = isBodyAProjectile ? entityA : entityB;
                var targetEntity = isBodyAProjectile ? entityB : entityA;

                // destroy bullet.
                CommandBuffer.DestroyEntity(projectileEntity);

                if (Target.Exists(targetEntity)) { 

                // Decrease target HP
                var hp = Target[targetEntity];
                hp.Value -= DamageDealer[projectileEntity].Value;
                Target[targetEntity] = hp;
            }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return  new ProjectileDamageJob
            {
                    CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
                    DamageDealer = GetComponentDataFromEntity<Damage>(true),
                    Target = GetComponentDataFromEntity<Health>()
            }.Schedule(m_StepPhysicsWorldSystem.Simulation,
                      ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        }
    }
}