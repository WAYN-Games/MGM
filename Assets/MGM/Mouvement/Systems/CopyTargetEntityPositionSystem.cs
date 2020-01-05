using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MovementSystemGroup))]
public class CopyTargetEntityPositionSystem : JobComponentSystem
{

    private EntityQuery m_Query;
    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadOnly<TrackedTargetReference>()
            }
        };
        m_Query = GetEntityQuery(query);
    
    }

    [BurstCompile]
    struct MouveSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<Translation> Translation;
        [ReadOnly] public ArchetypeChunkComponentType<TrackedTargetReference> TrackedTargetReference;
        [ReadOnly] public ComponentDataFromEntity<LocalToWorld> OtherEntitiesLocalToWorld;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkTranslations = chunk.GetNativeArray(Translation);
            var chunkTrackedTargetReferences = chunk.GetNativeArray(TrackedTargetReference);

            for (var i = 0; i < chunk.Count; i++)
            {
                var translation = chunkTranslations[i];
                translation.Value = OtherEntitiesLocalToWorld[chunkTrackedTargetReferences[i].Value].Position ;
                chunkTranslations[i] = translation;
            }
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new MouveSystemJob()
        {
            Translation = GetArchetypeChunkComponentType<Translation>(false),
            TrackedTargetReference = GetArchetypeChunkComponentType<TrackedTargetReference>(true),
            OtherEntitiesLocalToWorld = GetComponentDataFromEntity<LocalToWorld>(true)
        };


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(m_Query, inputDependencies);
    }
}