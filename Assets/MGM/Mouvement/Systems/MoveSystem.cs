using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateInGroup(typeof(MouvementSystemGroup))]
[UpdateAfter(typeof(GroundInfoCollectionSystem))]
public class MoveSystem : JobComponentSystem
{

    private EntityQuery m_Query;
    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<PhysicsVelocity>(),
                ComponentType.ReadWrite<PhysicsMass>(),
                ComponentType.ReadOnly<MovementDirection>(),
                ComponentType.ReadOnly<MovementSpeed>(),
                ComponentType.ReadOnly<GroundInfo>() }
        };
        m_Query = GetEntityQuery(query);
    
    }

    [BurstCompile]
    struct MouveSystemJob : IJobChunk
    {
        public ArchetypeChunkComponentType<PhysicsVelocity> Velocity;
        public ArchetypeChunkComponentType<PhysicsMass> Mass;
        [ReadOnly] public ArchetypeChunkComponentType<MovementDirection> Direction;
        [ReadOnly] public ArchetypeChunkComponentType<MovementSpeed> Speed;
        [ReadOnly] public ArchetypeChunkComponentType<GroundInfo> IsOnGround;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkVelocities = chunk.GetNativeArray(Velocity);
            var chunkMasses = chunk.GetNativeArray(Mass);
            var chunkDirections = chunk.GetNativeArray(Direction);
            var chunkSpeeds = chunk.GetNativeArray(Speed);
            var chunkIsOnGrounds = chunk.GetNativeArray(IsOnGround);

            for (var i = 0; i < chunk.Count; i++)
            {

                var mass = chunkMasses[i];
                mass.InverseInertia = new float3(0);
                chunkMasses[i] = mass;

                if (!chunkIsOnGrounds[i].IsGrounded) return;
                var velocity = chunkVelocities[i];
                var direction = chunkDirections[i].Value;

                if (new float3(0, 0, 0).Equals(direction)) return;

                var speed = chunkSpeeds[i].Value;

                direction = normalize(direction) * speed;
                direction.y = velocity.Linear.y;
                
                chunkVelocities[i] = new PhysicsVelocity
                {
                    Angular = velocity.Angular,
                    Linear = direction
                };

            }
        }
    }

   
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new MouveSystemJob()
        {
            Velocity = GetArchetypeChunkComponentType<PhysicsVelocity>(false),
            Mass = GetArchetypeChunkComponentType<PhysicsMass>(false),
            Direction = GetArchetypeChunkComponentType<MovementDirection>(true),
            Speed = GetArchetypeChunkComponentType<MovementSpeed>(true),
            IsOnGround = GetArchetypeChunkComponentType<GroundInfo>(true)
        };


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(m_Query, inputDependencies);
    }
}