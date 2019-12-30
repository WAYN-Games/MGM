using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MouvementSystemGroup))]
[UpdateAfter(typeof(GroundInfoCollectionSystem))]
public class JumpSystem : JobComponentSystem
{

    private EntityQuery m_JumpQuery;
    private EntityQuery m_ResetJumpCountQuery;
    protected override void OnCreate()
    {
        var resetJumpCountQuery = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<JumpCount>(),
                ComponentType.ReadOnly<GroundInfo>() }
        };
        m_ResetJumpCountQuery = GetEntityQuery(resetJumpCountQuery);

        var jumpQuery = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<PhysicsVelocity>(),
                ComponentType.ReadWrite<JumpCount>(),
                ComponentType.ReadWrite<JumpTrigger>(),
                ComponentType.ReadOnly<JumpForce>(),
                ComponentType.ReadOnly<MaxJumpCount>()}
        };
        m_JumpQuery = GetEntityQuery(jumpQuery);
    }

    [BurstCompile]
    struct ResetJumpCountSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<JumpCount> JumpCount;
        [ReadOnly] public ArchetypeChunkComponentType<GroundInfo> IsOnTheGround;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkJumpCount = chunk.GetNativeArray(JumpCount);
            var chunkIsOnGround = chunk.GetNativeArray(IsOnTheGround);
     
            for (var i = 0; i < chunk.Count; i++)
            {
                var jumpCount = chunkJumpCount[i].Value;
                var isOnGround = chunkIsOnGround[i].IsGrounded;
                chunkJumpCount[i] = new JumpCount() { Value = math.select(jumpCount, 0, isOnGround) };
            }
        }
    }


    [BurstCompile]
    struct JumpSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<PhysicsVelocity> Velocity;
        public ArchetypeChunkComponentType<JumpCount> JumpCount;
        public ArchetypeChunkComponentType<JumpTrigger> JumpTrigger;
        [ReadOnly] public ArchetypeChunkComponentType<JumpForce> JumpForce;
        [ReadOnly] public ArchetypeChunkComponentType<MaxJumpCount> MaxJumpCount;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkVelocities = chunk.GetNativeArray(Velocity);
            var chunkJumpCount = chunk.GetNativeArray(JumpCount);
            var chunkJumpForces = chunk.GetNativeArray(JumpForce);
            var chunkMaxJumpCounts = chunk.GetNativeArray(MaxJumpCount);
            var chunkJumpTrigger = chunk.GetNativeArray(JumpTrigger);

            for (var i = 0; i < chunk.Count; i++)
            {
                if (!chunkJumpTrigger[i].Value) return;

                chunkJumpTrigger[i] = new JumpTrigger() { Value = false };
       
                chunkJumpCount[i] = new JumpCount() { Value = chunkJumpCount[i].Value + 1 };

                if (chunkJumpCount[i].Value > chunkMaxJumpCounts[i].Value) return;

                chunkVelocities[i] = new PhysicsVelocity
                {
                    Angular = chunkVelocities[i].Angular,
                    Linear = math.select(chunkVelocities[i].Linear + new float3(0, chunkJumpForces[i].Value, 0),
                                            new float3(chunkVelocities[i].Linear.x, chunkJumpForces[i].Value, chunkVelocities[i].Linear.z),
                                            chunkJumpForces[i].IsAbsolute)
                };

            }
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var resetJumpCountJob = new ResetJumpCountSystemJob()
        {
            JumpCount = GetArchetypeChunkComponentType<JumpCount>(false),
            IsOnTheGround = GetArchetypeChunkComponentType<GroundInfo>(true)
        };

        inputDependencies = resetJumpCountJob.Schedule(m_ResetJumpCountQuery, inputDependencies);

        var jumpJob = new JumpSystemJob()
        {
            Velocity = GetArchetypeChunkComponentType<PhysicsVelocity>(false),
            JumpCount = GetArchetypeChunkComponentType<JumpCount>(false),
            JumpTrigger = GetArchetypeChunkComponentType<JumpTrigger>(false),
            JumpForce = GetArchetypeChunkComponentType<JumpForce>(true),
            MaxJumpCount = GetArchetypeChunkComponentType<MaxJumpCount>(true)
        };

        // Now that the job is set up, schedule it to be run. 
        return jumpJob.Schedule(m_JumpQuery, inputDependencies);
    }
}