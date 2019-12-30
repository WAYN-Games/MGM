using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputSystem : PlayerInputSystem
{
    private EntityQuery m_Query;
    private float3 m_InputDirection;
    private float3 m_LastFrameInputDirection;

    private Camera m_Camera;

    protected override void OnCreate()
    {

        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Frozen) },
            All = new ComponentType[] {
                ComponentType.ReadWrite<MovementDirection>(),
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
        public float3 InputDirection;
        public float3 CameraFoward;
        public float3 CameraRight;
        public ArchetypeChunkComponentType<MovementDirection> Direction;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkDirections = chunk.GetNativeArray(Direction);
           
            for (var i = 0; i < chunk.Count; i++)
            {

                var direction = InputDirection.z * CameraFoward + InputDirection.x * CameraRight;
                chunkDirections[i] = new MovementDirection
                {
                    Value = direction
                };

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        // Check the input changed.
        if (m_LastFrameInputDirection.Equals(m_InputDirection)) return inputDependencies;

        // Cache the input for net frame comaprison.
        m_LastFrameInputDirection = m_InputDirection;
         
        // Input changed so we schedule the job.
        var job = new MovementInputSystemJob()
        {
            InputDirection = m_InputDirection,
            CameraFoward = new float3(m_Camera.transform.forward.x, 0, m_Camera.transform.forward.z),
            CameraRight = new float3(m_Camera.transform.right.x, 0, m_Camera.transform.right.z),
            Direction = GetArchetypeChunkComponentType<MovementDirection>(false)
        };
        return  job.Schedule(m_Query,inputDependencies);
    }
    
    internal override void ActionPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 InputMouvementDirection = ctx.ReadValue<Vector2>();

        m_InputDirection = new float3(InputMouvementDirection.x, 0, InputMouvementDirection.y);
    }
}
