using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(CollisionTrigerHierarchyDestructionSystem))]
public class DisableHierarchySystem : JobComponentSystem
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
                    ComponentType.ReadOnly<DisableHierarchy>(),
                    ComponentType.ReadOnly<Child>()
            }
        });

        m_RootsGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {   
                    ComponentType.ReadOnly<DisableHierarchy>()
            }
        });

    }

    [BurstCompile]
    struct DisableChilds : IJobChunk
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
                    RecursiveChildsDisable(children[j].Value,index);
                }
            }
        }


        void RecursiveChildsDisable(Entity entity, int index)
        {
            if (ChildFromEntity.Exists(entity))
            {
                var children = ChildFromEntity[entity];
                for (int i = 0; i < children.Length; i++)
                {
                    RecursiveChildsDisable(children[i].Value, index);
                }
            }
            EntityCommandBuffer.AddComponent<Disabled>(index, entity);
            EntityCommandBuffer.RemoveComponent<DisableHierarchy>(index, entity);
        }
    }

    [BurstCompile]
    struct DisableHierarchyRootJob : IJobForEachWithEntity<DisableHierarchy>
    {
        public EntityCommandBuffer.Concurrent EntityCommandBuffer;

        public void Execute(Entity entity, int index, [ReadOnly] ref DisableHierarchy c0)
        {
            EntityCommandBuffer.AddComponent<Disabled>(index, entity);
            EntityCommandBuffer.RemoveComponent<DisableHierarchy>(index, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var childType = GetArchetypeChunkBufferType<Child>(true);
        var childFromEntity = GetBufferFromEntity<Child>(true);

        var disableChilds = new DisableChilds
        {
            ChildType = childType,
            ChildFromEntity = childFromEntity,
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        };
        var disableChildsJobHandle = disableChilds.Schedule(m_ChildRootGroup, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(disableChildsJobHandle);

        var disableHierarchyRootJob = new DisableHierarchyRootJob()
        {
            EntityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        };
        var disableHierarchyRootJobHandle = disableHierarchyRootJob.Schedule(m_RootsGroup, disableChildsJobHandle);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(disableHierarchyRootJobHandle);

        return disableHierarchyRootJobHandle;
    }
}
