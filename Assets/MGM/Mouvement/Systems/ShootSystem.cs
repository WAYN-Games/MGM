using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MovementSystemGroup))]
public class ShootSystem : JobComponentSystem
{

    private EntityQuery m_ShootQuery;
    private EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var jumpQuery = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<ShootTrigger>(),
                ComponentType.ReadOnly<ProjectileEntityReference>(),
                ComponentType.ReadOnly<LocalToWorld>(),
            }
        };
        m_ShootQuery = GetEntityQuery(jumpQuery);
    }

    [BurstCompile]
    struct ShootSystemJob : IJobChunk
    {
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;
        public ArchetypeChunkComponentType<ShootTrigger> ShootTrigger;
        [ReadOnly] public ArchetypeChunkComponentType<ProjectileEntityReference> ProjectileEntityReference;
        [ReadOnly] public ArchetypeChunkComponentType<LocalToWorld> LocalToWorld;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkProjectileEntityReference = chunk.GetNativeArray(ProjectileEntityReference);
            var chunkShootTrigger = chunk.GetNativeArray(ShootTrigger);
            var chunkLocalToWorld = chunk.GetNativeArray(LocalToWorld);

            for (var i = 0; i < chunk.Count; i++)
            {
                if (!chunkShootTrigger[i].Value) continue;

                Entity projectile = InstantiateEntityAtLocalToWorld(chunkIndex, chunkProjectileEntityReference[i], chunkLocalToWorld[i]);
                EntityCommandBuffer.SetComponent(chunkIndex, projectile, new MovementDirection() { Value = chunkLocalToWorld[i].Forward });
            }
        }

        private Entity InstantiateEntityAtLocalToWorld(int chunkIndex, ProjectileEntityReference chunkProjectileEntityReference, LocalToWorld chunkLocalToWorld)
        {
            Entity entity = EntityCommandBuffer.Instantiate(chunkIndex, chunkProjectileEntityReference.Value);
            EntityCommandBuffer.SetComponent(chunkIndex, entity, new Translation() { Value = chunkLocalToWorld.Position });
            EntityCommandBuffer.SetComponent(chunkIndex, entity, new Rotation() { Value = chunkLocalToWorld.Rotation });
            // Default scale is 1, setting it to 0 avoid weird visual effect, making it effectively invisible until it get the propoer scale set by the scale component.
            EntityCommandBuffer.SetComponent(chunkIndex, entity, new LocalToWorld() { Value = math.mul(float4x4.Scale(0f), chunkLocalToWorld.Value) });
            return entity;
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var shootJob = new ShootSystemJob()
        {
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            ShootTrigger = GetArchetypeChunkComponentType<ShootTrigger>(false),
            ProjectileEntityReference = GetArchetypeChunkComponentType<ProjectileEntityReference>(true),
            LocalToWorld = GetArchetypeChunkComponentType<LocalToWorld>(true)
        }.Schedule(m_ShootQuery, inputDependencies);
        m_EntityCommandBufferSystem.AddJobHandleForProducer(shootJob);
        return shootJob;
    }
}