using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(MouvementSystemGroup))]
[UpdateAfter(typeof(MoveSystem))]
public class FaceMovementDirectionSystem : JobComponentSystem
{

    private EntityQuery m_Query;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadOnly<MovementDirection>(),
                ComponentType.ReadOnly<AlwaysFaceMovementDirection>()
            }
        };
        m_Query = GetEntityQuery(query);
    }

    [BurstCompile]
    struct FaceHeadingSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<Rotation> Rotation;
        [ReadOnly] public ArchetypeChunkComponentType<MovementDirection> MovementDirection;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkMovementDirections = chunk.GetNativeArray(MovementDirection);
            var chunkRotations = chunk.GetNativeArray(Rotation);

            for (var i = 0; i < chunk.Count; i++)
            {
                var direction = chunkMovementDirections[i].Value;
                if (math.length(direction) == 0) break;

                var rotation = chunkRotations[i] ;
                rotation.Value = Quaternion.LookRotation(direction, Vector3.up);
                chunkRotations[i] = rotation;
            }
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new FaceHeadingSystemJob()
        {
            Rotation = GetArchetypeChunkComponentType<Rotation>(false),
            MovementDirection = GetArchetypeChunkComponentType<MovementDirection>(true)
        };

        return job.Schedule(m_Query, inputDependencies);
    }
}