using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MouvementSystemGroup))]
public class GroundInfoCollectionSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private BuildPhysicsWorld m_physicsWorldSystem;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<GroundInfo>() ,
                ComponentType.ReadOnly<Translation>() ,
                ComponentType.ReadOnly<PhysicsCollider>() }
        };
        m_Query = GetEntityQuery(query);
        m_physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
    }


    [BurstCompile]
    struct IsOnGroundCheckSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<GroundInfo> GroundInfo;

        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public ArchetypeChunkComponentType<PhysicsCollider> Collider;
        [ReadOnly] public ArchetypeChunkComponentType<Translation> ColliderPosition;

        public unsafe void  Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var colliders = chunk.GetNativeArray(Collider);
            var groundInfo = chunk.GetNativeArray(GroundInfo);
            var colliderPositions = chunk.GetNativeArray(ColliderPosition);

            for (var i = 0; i < chunk.Count; i++)
            {
                float3 position = colliderPositions[i].Value;

                ColliderCastInput colliderCastInput = new ColliderCastInput()
                {
                    Start = position,
                    End = position - new float3(0, groundInfo[i].GroundCheckDistance, 0) ,
                    Collider = colliders[i].ColliderPtr
                };

                ColliderCastHit hit;

                groundInfo[i] = new GroundInfo
                {
                    IsGrounded = CollisionWorld.CastCollider(colliderCastInput, out hit),
                    GroundCheckDistance = groundInfo[i].GroundCheckDistance,
                    GroundNormal = hit.SurfaceNormal
                };
            }
        }
    }



    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new IsOnGroundCheckSystemJob()
        {
            CollisionWorld = m_physicsWorldSystem.PhysicsWorld.CollisionWorld,
            Collider = GetArchetypeChunkComponentType<PhysicsCollider>(true),
            GroundInfo = GetArchetypeChunkComponentType<GroundInfo>(false),
            ColliderPosition = GetArchetypeChunkComponentType<Translation>(true)
        };


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(m_Query, inputDependencies);
    }
}