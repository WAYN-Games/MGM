using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MovementSystemGroup))]
public class CollectContactInfosSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private BuildPhysicsWorld m_physicsWorldSystem;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] {
                ComponentType.ReadWrite<ContactInfos>() ,
                ComponentType.ReadOnly<PhysicsCollider>() ,
                ComponentType.ReadOnly<LocalToWorld>()
            }
        };
        m_Query = GetEntityQuery(query);
        m_physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
    }


    struct CollectContactInfosSystemJob : IJobChunk
    {
        public ArchetypeChunkBufferType<ContactInfos> ContactInfos;
        [ReadOnly] public NativeSlice<RigidBody> Bodies;

        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public ArchetypeChunkComponentType<PhysicsCollider> Collider;
        [ReadOnly] public ArchetypeChunkComponentType<LocalToWorld> LocalToWorld;
        

        public unsafe void  Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {

            BufferAccessor<ContactInfos> chunkContactInfosBuffers = chunk.GetBufferAccessor(ContactInfos);

            var colliders = chunk.GetNativeArray(Collider);
            var colliderLocalToWorlds = chunk.GetNativeArray(LocalToWorld);

            for (var i = 0; i < chunk.Count; i++)
            {
                DynamicBuffer<ContactInfos> contactInfosBuffer =  chunkContactInfosBuffers[i];

                contactInfosBuffer.Clear();

                float3 position = colliderLocalToWorlds[i].Position;

                ColliderCastInput colliderCastInput = new ColliderCastInput()
                {
                    Start = position,
                    End = position,
                    Collider = colliders[i].ColliderPtr
                };

                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                if(CollisionWorld.CastCollider(colliderCastInput, ref hits))
                {
                    foreach(var hit in hits)
                    {
                        if (Bodies[hit.RigidBodyIndex].Collider.GetUnsafePtr() == colliderCastInput.Collider) continue;

                        contactInfosBuffer.Add(new ContactInfos()
                        {
                            Entity = Bodies[hit.RigidBodyIndex].Entity,
                            Contact = hit
                        });
                    }
                }
                hits.Dispose();
            }

            
        }
    }



    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {

        var job = new CollectContactInfosSystemJob()
        {
            CollisionWorld = m_physicsWorldSystem.PhysicsWorld.CollisionWorld,
            Bodies = m_physicsWorldSystem.PhysicsWorld.Bodies,
            ContactInfos = GetArchetypeChunkBufferType<ContactInfos>(false),
            Collider = GetArchetypeChunkComponentType<PhysicsCollider>(true),
            LocalToWorld = GetArchetypeChunkComponentType<LocalToWorld>(true)
        };


        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(m_Query, inputDependencies);
    }
}