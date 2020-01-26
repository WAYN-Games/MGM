using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(CollisionTrigerHierarchyDestructionSystem))]
public class DestroyHierarchySystem : JobComponentSystem
{
    private EntityQuery m_ChildRootGroup;
    private EntityQuery m_RootsGroup;
    private EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_ChildRootGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                    ComponentType.ReadOnly<DestroyHierarchy>(),
                    ComponentType.ReadOnly<Child>()
            }
        });

        m_RootsGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {   
                    ComponentType.ReadOnly<DestroyHierarchy>()
            }
        });

    }

    [BurstCompile]
    struct DestroyChilds : IJobChunk
    {
        [ReadOnly] public ArchetypeChunkBufferType<Child> ChildType;
        [ReadOnly] public BufferFromEntity<Child> ChildFromEntity;
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;

        public void Execute(ArchetypeChunk chunk, int index, int entityOffset)
        {
            var chunkChildren = chunk.GetBufferAccessor(ChildType);
            for (int i = 0; i < chunk.Count; i++)
            {
                var children = chunkChildren[i];
                for (int j = 0; j < children.Length; j++)
                {
                    RecursiveChildsDestroy(children[j].Value,index);
                }
            }
        }


        void RecursiveChildsDestroy(Entity entity, int index)
        {
            EntityCommandBuffer.DestroyEntity(index,entity);
            if (ChildFromEntity.Exists(entity))
            {
                var children = ChildFromEntity[entity];
                for (int i = 0; i < children.Length; i++)
                {
                    RecursiveChildsDestroy(children[i].Value, index);
                }
            }
        }
    }

    [BurstCompile]
    struct DestroyHierarchyRootJob : IJobForEachWithEntity<DestroyHierarchy>
    {
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;

        public void Execute(Entity entity, int index, [ReadOnly] ref DestroyHierarchy c0)
        {
            EntityCommandBuffer.DestroyEntity(index,entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var childType = GetArchetypeChunkBufferType<Child>(true);
        var childFromEntity = GetBufferFromEntity<Child>(true);

        var destroyChilds = new DestroyChilds
        {
            ChildType = childType,
            ChildFromEntity = childFromEntity,
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        };
        var destroyChildsJobHandle = destroyChilds.Schedule(m_ChildRootGroup, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(destroyChildsJobHandle);

        var destroyHierarchyRootJob = new DestroyHierarchyRootJob()
        {
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        };
        var destroyHierarchyRootJobHandle = destroyHierarchyRootJob.Schedule(m_RootsGroup, destroyChildsJobHandle);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(destroyHierarchyRootJobHandle);

        return destroyHierarchyRootJobHandle;
    }
}
