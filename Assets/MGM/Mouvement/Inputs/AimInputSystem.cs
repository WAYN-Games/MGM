using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimInputSystem : PlayerInputSystem
{
    private EntityQuery m_Query;

    private Camera m_Camera;
    private Vector2 m_PointerPosition;

    protected override void OnCreate()
    {

        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<AimPosition>(),
                ComponentType.ReadOnly<PlayerControled>()
            }
        };
        m_Query = GetEntityQuery(query);

    }


    protected override void OnStartRunning()
    {
        m_Camera = Camera.main;
        base.OnStartRunning();
    }

    [BurstCompile]
    struct MovementInputSystemJob : IJobChunk
    {
        public float3 AimInput;
        public ArchetypeChunkComponentType<AimPosition> AimPosition;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkAimPositions = chunk.GetNativeArray(AimPosition);
           
            for (var i = 0; i < chunk.Count; i++)
            {
                var aimPosition = chunkAimPositions[i];
                aimPosition.Value = AimInput;
                chunkAimPositions[i] = aimPosition;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new MovementInputSystemJob()
        {
            AimInput = MouseToWorldPosition(m_PointerPosition,m_Camera),
            AimPosition = GetArchetypeChunkComponentType<AimPosition>(false)
        };
        return  job.Schedule(m_Query,inputDependencies);
    }
    
    internal override void ActionPerformed(InputAction.CallbackContext ctx)
    {
        m_PointerPosition = ctx.ReadValue<Vector2>();
    }

    internal float3 MouseToWorldPosition(Vector2 mousePos, Camera cam)
    {

        RaycastInput RaycastInput = new RaycastInput
        {
            Start = cam.transform.position,
            End = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane)),

            Filter = CollisionFilter.Default
        };

        CollisionWorld w = World.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;

        Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit();

        RayCastJobUtils.SingleRayCast(w, RaycastInput, ref hit);

        return hit.Fraction == 0 ? RaycastInput.End : hit.Position;

    }

}
