using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MouvementSystemGroup))]
[UpdateAfter(typeof(FaceMovementDirectionSystem))]
public class AimSystem : JobComponentSystem
{

    private EntityQuery m_Query;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadOnly<LocalToWorld>(),
                ComponentType.ReadOnly<AimPosition>()
            },
            Any = new ComponentType[] {
                ComponentType.ReadOnly<Parent>(),
                ComponentType.ReadOnly<Entity>()
            }
        };
        m_Query = GetEntityQuery(query);
    }

    [BurstCompile]
    struct AimSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<Rotation> Rotation;
        [ReadOnly] public ArchetypeChunkComponentType<AimPosition> AimPosition;
        [ReadOnly] public ArchetypeChunkComponentType<LocalToWorld> LocalToWorld;
        [ReadOnly] public ArchetypeChunkComponentType<Parent> Parent;
        [ReadOnly] public ComponentDataFromEntity<LocalToWorld> ParentLocalToWorld;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkRotations = chunk.GetNativeArray(Rotation);
            var chunkAimPositions = chunk.GetNativeArray(AimPosition);
            var chunkLocalToWorlds = chunk.GetNativeArray(LocalToWorld);
            var chunkParents = chunk.GetNativeArray(Parent);

            bool hasParent = chunk.Has(Parent);

            for (var i = 0; i < chunk.Count; i++)
            {
                var direction = chunkAimPositions[i].Value - chunkLocalToWorlds[i].Position;
                direction.y = 0;

                var rotation = chunkRotations[i];

                rotation.Value = hasParent ? 
                    math.mul(math.inverse(ParentLocalToWorld[chunkParents[i].Value].Rotation), quaternion.LookRotationSafe(direction, ParentLocalToWorld[chunkParents[i].Value].Up))
                    :
                    quaternion.LookRotationSafe(direction, chunkLocalToWorlds[i].Up);
                
                chunkRotations[i] = rotation;
            }
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new AimSystemJob()
        {
            Rotation = GetArchetypeChunkComponentType<Rotation>(false),
            AimPosition = GetArchetypeChunkComponentType<AimPosition>(true),
            LocalToWorld = GetArchetypeChunkComponentType<LocalToWorld>(true),
            Parent = GetArchetypeChunkComponentType<Parent>(true),
            ParentLocalToWorld = GetComponentDataFromEntity<LocalToWorld>(true)
        };

        return job.Schedule(m_Query, inputDependencies);
    }
}
