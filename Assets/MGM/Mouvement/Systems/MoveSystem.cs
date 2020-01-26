using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(MovementSystemGroup))]
[UpdateAfter(typeof(CollectContactInfosSystem))]
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
                ComponentType.ReadOnly<MovementSpeed>()
            }
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

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkVelocities = chunk.GetNativeArray(Velocity);
            var chunkMasses = chunk.GetNativeArray(Mass);
            var chunkDirections = chunk.GetNativeArray(Direction);
            var chunkSpeeds = chunk.GetNativeArray(Speed);

            for (var i = 0; i < chunk.Count; i++)
            {

                var mass = chunkMasses[i];
                mass.InverseInertia = new float3(0);
                chunkMasses[i] = mass;

                // if (!chunkIsOnGrounds[i].IsGrounded) continue;
                var velocity = chunkVelocities[i];
                var direction = chunkDirections[i].Value;

                if (new float3(0, 0, 0).Equals(direction)) continue;

                var speed = chunkSpeeds[i].Value;
                ;
                direction = speed * math.select(math.normalizesafe(direction), direction, math.length(direction)>1);
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
            Speed = GetArchetypeChunkComponentType<MovementSpeed>(true)
        };


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(m_Query, inputDependencies);
    }
}