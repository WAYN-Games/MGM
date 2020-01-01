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
                float3 direction = ComputeAimDirectionRelativeToEntity( chunkAimPositions[i],  chunkLocalToWorlds[i]);

                var rotation = chunkRotations[i];

                rotation.Value = hasParent ?
                    ComputeRotationRelativeToParent(chunkParents[i], i, direction)
                    :
                    ComputeRotationRelativeToWorld(chunkLocalToWorlds[i], i, direction);

                chunkRotations[i] = rotation;
            }
        }

        private static float3 ComputeAimDirectionRelativeToEntity(AimPosition aimPositions, LocalToWorld localToWorlds)
        {
            var direction = aimPositions.Value - localToWorlds.Position;
            direction.y = 0;
            return direction;
        }

        private static quaternion ComputeRotationRelativeToWorld(LocalToWorld localToWorlds, int i, float3 direction)
        {
            return quaternion.LookRotationSafe(direction, localToWorlds.Up);
        }

        private quaternion ComputeRotationRelativeToParent(Parent parent, int i, float3 direction)
        {
            return math.mul(
                math.inverse(ParentLocalToWorld[parent.Value].Rotation),
                quaternion.LookRotationSafe(direction, ParentLocalToWorld[parent.Value].Up)
                );
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
