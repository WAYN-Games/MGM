using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MovementSystemGroup))]
[UpdateAfter(typeof(CollectContactInfosSystem))]
public class JumpSystem : JobComponentSystem
{

    private EntityQuery m_JumpQuery;
    private EntityQuery m_ResetJumpCountQuery;
    protected override void OnCreate()
    {
        EntityQueryDesc resetJumpCountQuery = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<JumpCount>()}
        };
        m_ResetJumpCountQuery = GetEntityQuery(resetJumpCountQuery);

        EntityQueryDesc jumpQuery = new EntityQueryDesc
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

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<JumpCount> chunkJumpCount = chunk.GetNativeArray(JumpCount);

            for (int i = 0; i < chunk.Count; i++)
            {
                chunkJumpCount[i] = new JumpCount() { Value = 0 };
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
            NativeArray<PhysicsVelocity> chunkVelocities = chunk.GetNativeArray(Velocity);
            NativeArray<JumpCount> chunkJumpCount = chunk.GetNativeArray(JumpCount);
            NativeArray<JumpForce> chunkJumpForces = chunk.GetNativeArray(JumpForce);
            NativeArray<MaxJumpCount> chunkMaxJumpCounts = chunk.GetNativeArray(MaxJumpCount);
            NativeArray<JumpTrigger> chunkJumpTrigger = chunk.GetNativeArray(JumpTrigger);

            for (int i = 0; i < chunk.Count; i++)
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

        ResetJumpCountSystemJob resetJumpCountJob = new ResetJumpCountSystemJob()
        {
            JumpCount = GetArchetypeChunkComponentType<JumpCount>(false)
        };

        inputDependencies = resetJumpCountJob.Schedule(m_ResetJumpCountQuery, inputDependencies);

        JumpSystemJob jumpJob = new JumpSystemJob()
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
